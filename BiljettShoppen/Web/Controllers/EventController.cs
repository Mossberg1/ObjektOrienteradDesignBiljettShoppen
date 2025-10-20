using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Entities.Base;
using Models.Enums;
using Web.Models;

namespace Web.Controllers;

public class EventController : Controller
{
    public EventController() {}

    // TODO: Example
    [HttpGet]
    public async Task<IActionResult> Browse()
    {
        var fakeEvents = new List<Event>
        {
            new()
            {
                Id = 6,
                Name = "Rockkonsert i Globen",
                Date = new DateOnly(2025, 11, 5),
                ArenaNavigation = new Arena { Name = "Avicii Arena" }
            },
            new()
            {
                Id = 5,
                Name = "Stand-up Comedy Night",
                Date = new DateOnly(2025, 11, 22),
                ArenaNavigation = new Arena { Name = "Cirkus" }
            },
            new()
            {
                Id = 4,
                Name = "Klassisk Musikgala",
                Date = new DateOnly(2025, 7, 12),
                ArenaNavigation = new Arena { Name = "Konserthuset Stockholm" }
            },
            new()
            {
                Id = 3,
                Name = "Popfestivalen 2025",
                Date = new DateOnly(2025, 9, 28),
                ArenaNavigation = new Arena { Name = "Tele2 Arena" }
            },
            new()
            {
                Id = 2,
                Name = "Teater: Hamlet",
                Date = new DateOnly(2026, 1, 10),
                ArenaNavigation = new Arena { Name = "Dramaten" }
            },
            new()
            {
                Id = 1,
                Name = "Magishow med Joe Labero",
                ArenaNavigation = new Arena { Name = "Hamburger Börs" }
            }
        };

        return View(fakeEvents);
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