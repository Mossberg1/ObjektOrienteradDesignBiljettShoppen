using Application.Features.Events.Browse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Browse()
    {
        var query = new BrowseEventsQuery();
        var events = await _mediator.Send(query);

        return View(events);
    }

    // TODO: Example
    [HttpGet("Event/Buy/{id:int}")]
    public async Task<IActionResult> Buy([FromRoute] int id)
    {
        var eventDetails = new Event
        {
            Id = id,
            Name = "Rockkonsert i Globen",
            Date = new DateOnly(2025, 10, 15),
            StartTime = new TimeOnly(20, 0, 0),
            Price = 795,
            ArenaNavigation = new Arena { Name = "Avicii Arena" }
        };

        var seats = new List<BookableSpace>();
        var seatIdCounter = 1;
        var maxRows = 10;
        var maxSeatsInRow = 20;

        // Generate some fake seats
        for (var row = 1; row <= maxRows; row++)
        {
            for (var num = 1; num <= maxSeatsInRow; num++)
            {
                var isBooked = (row % 4 == 0 && num % 3 == 0);
                var seatType = (row > 7) ? SeatType.Bench : SeatType.Chair;

                seats.Add(new Seat
                {
                    Id = seatIdCounter++,
                    RowNumber = row,
                    ColNumber = num,
                    Type = seatType, 
                    IsBooked = isBooked
                });
            }
        }
        
        var viewModel = new BuyTicketViewModel
        {
            Event = eventDetails,
            Seats = seats,
            MaxRow = maxRows,
            MaxSeatNumber = maxSeatsInRow
        };

        return View(viewModel);
    }
    
    // TODO: Example
    [HttpPost]
    public async Task<IActionResult> ConfirmPurchase(int eventId, List<int> selectedSeats)
    {
        if (selectedSeats == null || !selectedSeats.Any())
        {
            return RedirectToAction("Buy", new { id = eventId });
        }
        return RedirectToAction("Index", "Home");
    }
}