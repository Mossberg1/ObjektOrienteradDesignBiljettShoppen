using Application.Features.Booking.Create;
using Application.Features.Events.Browse;
using Application.Features.Events.ViewSeats;
using Application.Features.Payments.PayBooking;
using Application.Features.Seats.GetSelectedSeats;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Base;
using Models.Enums;
using Web.Models;

namespace Web.Controllers;

public class EventController : Controller
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Browse(
        [FromQuery] string? searchWord, 
        [FromQuery] EventType? type,
        [FromQuery] DateOnly? FromDate,
        [FromQuery] DateOnly? ToDate,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 24
    )
    {
        var query = new BrowseEventsQuery(searchWord, type, FromDate, ToDate, pageNumber, pageSize);
        var events = await _mediator.Send(query);

        return View(events);
    }
    
    [HttpGet("[controller]/Booking/{eventId:int}")]
    public async Task<IActionResult> Booking([FromRoute] int eventId)
    {
        var query = new ViewSeatsQuery(eventId);
        var ev = await _mediator.Send(query);
        if (ev == null)
            return NotFound();
        
        // TODO: Bryt ut byggande av viewmodel till egen klass.
        var seats = ev.SeatLayoutNavigation.SeatsNavigation;
        var seatIds = seats.Select(s => s.Id).ToArray();
        
        var tickets = await _mediator.Send(new GetSelectedSeatTicketsQuery(seatIds));
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
    
    [HttpGet("[controller]/Pay")]
    public async Task<IActionResult> Pay(
        [FromQuery] int[]? selectedSeats
    )
    {
        if (selectedSeats == null)
            return BadRequest();

        var tickets = await _mediator.Send(new GetSelectedSeatTicketsQuery(selectedSeats));
        var totalPrice = tickets.Sum(s => s.Price);
        var booking = await _mediator.Send(new CreateBookingCommand(totalPrice, tickets));
        
        var viewModel = new PayViewModel(booking.ReferenceNumber, booking.ReferenceNumber, booking.TotalPrice);
        
        return View(viewModel);
    }

    [HttpPost("[controller]/ProcessPayment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(
        [FromForm] string bookingReference,
        [FromForm] PaymentMethod paymentMethod
    )
    {
        if (string.IsNullOrEmpty(bookingReference))
            return BadRequest();

        var command = new PayBookingCommand(bookingReference, paymentMethod);
        var booking = await _mediator.Send(command);
        
        return RedirectToAction("Browse");
    }
}