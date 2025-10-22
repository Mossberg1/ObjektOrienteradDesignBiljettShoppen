using Application.Features.Events.Browse;
using Application.Features.Events.ViewSeats;
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
    
    private readonly IApplicationDbContext _context;

    public EventController(IMediator mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context; // TODO: Ta bort
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

    // TODO: Example
    [HttpGet("[controller]/Buy/{id:int}")]
    public async Task<IActionResult> Buy([FromRoute] int id)
    {
        var query = new ViewSeatsQuery(id);
        var ev = await _mediator.Send(query);
        if (ev == null)
            return NotFound();
        
        // TODO: Bryt ut byggande av viewmodel till egen klass.
        var seats = ev.SeatLayoutNavigation.SeatsNavigation;
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