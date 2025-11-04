using Application.Features.Bookings.Create;
using Application.Features.Bookings.SetBookingEmail;
using Application.Features.Bookings.UpdateConfirmationPath;
using Application.Features.Bookings.UpdateInvoicePath;
using Application.Features.Events.Browse;
using Application.Features.Events.ViewSeats;
using Application.Features.Payments.GenerateInvoice;
using Application.Features.Payments.PayBooking;
using Application.Features.Payments.TransactionConfirmation;
using Application.Features.Seats.GetSelectedSeats;
using Application.Interfaces;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Enums;
using System.Runtime.InteropServices;
using Web.Files;
using Web.Forms;
using Web.Models;

namespace Web.Controllers;

public class EventController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<EventController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _emailService;
    private readonly ICreditCardPaymentService _creditCardPaymentService;


    public EventController(
        IMediator mediator, 
        ILogger<EventController> logger, 
        IWebHostEnvironment env, 
        IEmailService emailService, 
        ICreditCardPaymentService creditCardPaymentService
    )
    {
        _mediator = mediator;
        _logger = logger;
        _env = env;
        _emailService = emailService;
        _creditCardPaymentService = creditCardPaymentService;
    }

    [HttpGet]
    public async Task<IActionResult> Browse(
        [FromQuery] string? searchWord,
        [FromQuery] EventType? type,
        [FromQuery] bool? isFamilyFriendly,
        [FromQuery] DateOnly? FromDate,
        [FromQuery] DateOnly? ToDate,
        [FromQuery] string? sortBy,
        [FromQuery] bool ascending = true,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 24
    )
    {
        var query = new BrowseReleasedEventsQuery(
            searchWord,
            type,
            isFamilyFriendly,
            FromDate,
            ToDate,
            sortBy,
            ascending,
            pageNumber,
            pageSize
        );
        var events = await _mediator.Send(query);

        return View(events);
    }

    [HttpGet("[controller]/Upcoming")]
    public async Task<IActionResult> Upcoming([FromQuery] BrowseCommingEventsQuery query) 
    {
        var result = await _mediator.Send(query);
        return View(result);
    }


    [HttpGet("[controller]/Booking/{eventId:int}")]
    public async Task<IActionResult> Booking([FromRoute] int eventId)
    {
        var query = new ViewSeatsQuery(eventId);
        var ev = await _mediator.Send(query);
        if (ev == null)
        {
            TempData["ErrorMessage"] = $"Inget event med {eventId} hittades.";
            return RedirectToAction("Browse");
        }

        // TODO: Bryt ut byggande av viewmodel till egen klass.
        var seats = ev.SeatLayoutNavigation.SeatsNavigation;
        var seatIds = seats.Select(s => s.Id).ToArray();

        var tickets = await _mediator.Send(new GetSelectedSeatTicketsQuery(seatIds, eventId));
        tickets = tickets.Where(t => t.EventId == ev.Id).ToList(); // TODO: TA BORT?

        var ticketBySpace = tickets.ToDictionary(t => t.BookableSpaceId, t => t.Price);

        foreach (var seat in seats)
        {
            if (ticketBySpace.TryGetValue(seat.Id, out var ticketPrice))
            {
                seat.Price = ticketPrice;
            }
        }


        var maxRows = ev.SeatLayoutNavigation.NumberOfRows;
        var maxSeatsInRow = ev.SeatLayoutNavigation.NumberOfCols;

        var viewModel = new BuySeatTicketViewModel
        {
            Event = ev,
            Seats = seats,
            MaxRow = maxRows,
            MaxSeatNumber = maxSeatsInRow
        };

        return View(viewModel);
    }

    [HttpGet("[controller]/Pay/{eventId:int}")]
    public async Task<IActionResult> Pay(
        [FromQuery] int[]? selectedSeats,
        [FromRoute] int eventId
    )
    {
        if (selectedSeats == null)
        {
            TempData["ErrorMessage"] = "Stolarna du har valt verkar inte finnas. Något kan ha ändrats medans du gjorde din bokning, försök igen.";
            return RedirectToAction("Browse");
        }

        var tickets = await _mediator.Send(new GetSelectedSeatTicketsQuery(selectedSeats, eventId));
        var totalPrice = tickets.Sum(s => s.Price);
        var booking = await _mediator.Send(new CreateBookingCommand(totalPrice, tickets));

        var viewModel = new PayViewModel(booking.ReferenceNumber, booking.ReferenceNumber, booking.TotalPrice);

        return View(viewModel);
    }

    [HttpPost("[controller]/ProcessPayment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(
        [FromForm] string bookingReference,
        [FromForm] PaymentMethod paymentMethod,
        [FromForm] CreditCardForm? creditCardForm,
        [FromForm] InvoiceForm? invoiceForm
    )
    {
        if (string.IsNullOrEmpty(bookingReference))
        {
            _logger.LogInformation("Bokningsreferens saknas.");
            TempData["ErrorMessage"] = "Något gick fel, försök igen.";
            return RedirectToAction("Browse");
        }

        if (creditCardForm == null && invoiceForm == null)
        {
            TempData["ErrorMessage"] = "Något gick fel vid val av betalmetod. Var vänlig försök igen";
            return RedirectToAction("Browse");
        }

        var bookingEmail = string.Empty;

        if (paymentMethod == PaymentMethod.CreditCard && creditCardForm != null)
        {
            var result = await _creditCardPaymentService.PayAsync(
                creditCardForm.CardNumber,
                creditCardForm.ExpiryDate,
                creditCardForm.Cvc,
                creditCardForm.Name
            );

            if (!result)
            {
                TempData["ErrorMessage"] = "Något gick fel, vänligen försök igen.";
                return RedirectToAction("Browse");
            }

            bookingEmail = creditCardForm.Email;

            _logger.LogInformation("Kort betalning lyckades!");
        }
        else if (paymentMethod == PaymentMethod.Invoice && invoiceForm != null)
        {
            bookingEmail = invoiceForm.Email;
            _logger.LogInformation("Faktura vald!");
        }
        else
        {
            _logger.LogWarning("Ogiltig betalningsmetod eller formulärdata.");
            TempData["ErrorMessage"] = "Något gick fel, vänligen försök igen.";
            return RedirectToAction("Browse");
        }

        var command = new PayBookingCommand(bookingReference, paymentMethod);
        var booking = await _mediator.Send(command);

        var setEmailCommand = new SetBookingEmailCommand(booking, bookingEmail);
        await _mediator.Send(setEmailCommand);

        var pdfBytes = CreatePdfConfirmation.GeneratePdf(booking);
        var confirmationPath = await FileSaver.SaveConfirmationAsync(pdfBytes, $"{booking.ReferenceNumber}_confirmation.pdf");

        var updatePdfPathCommand = new UpdateConfirmationPathCommand(booking, confirmationPath);
        await _mediator.Send(updatePdfPathCommand);

        if (paymentMethod == PaymentMethod.Invoice)
        {
            var invoiceBytes = CreateInvoice.GenerateInvoice(booking);
            var invoicePath = await FileSaver.SaveInvoiceAsync(invoiceBytes, $"{booking.ReferenceNumber}_invoice.pdf");
            var updateInvoicePathCommand = new UpdateInvoicePathCommand(booking, invoicePath);
            await _mediator.Send(updateInvoicePathCommand);
            await _emailService.SendBookingEmailWithFileAsync(booking.Email, bookingReference, invoiceBytes);
            _logger.LogInformation("Genererat faktura för bokning: " + booking.ReferenceNumber);
        }

        await _emailService.SendBookingEmailWithFileAsync(booking.Email, bookingReference, pdfBytes);

        return File(pdfBytes, "application/pdf", confirmationPath);
    }
}