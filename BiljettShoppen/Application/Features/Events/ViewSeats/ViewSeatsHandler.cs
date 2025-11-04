using Application.Utils;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.ViewSeats;
/// <summary>
/// Handler som tar emot <see cref="ViewSeatsQuery"/> via MediatR <br/>
/// och hämtar ett evenemang med platser sorterade per rad och kolumn.
/// </summary>
/// <remarks>
/// Hämtar information om arenan och dess seatlayout.
/// Returnerar null om evenemanget med det angivna Id:t inte finns.
/// </remarks>
/// <param name="request">Kommandot som innehåller Id för evenemanget som ska visas.</param>
/// <param name="cancellationToken">Token för att avbryta operationen.</param>
/// <returns>Evenemanget med dess seatlayout och arena, eller null om det inte finns.</returns>
public class ViewSeatsHandler : IRequestHandler<ViewSeatsQuery, Event?>
{
    private readonly IApplicationDbContext _dbContext;

    public ViewSeatsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> Handle(ViewSeatsQuery request, CancellationToken cancellationToken)
    {
        var ev = await _dbContext.Events
            .Include(e => e.SeatLayoutNavigation)
            .ThenInclude(sl => sl.SeatsNavigation
                .OrderBy(s => s.RowNumber)
                .ThenBy(s => s.ColNumber)
            )
            .Include(e => e.ArenaNavigation)
            .Include(e => e.TicketsNavigation)
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken: cancellationToken);

        if (ev == null)
            return null;

        foreach (var ticket in ev.TicketsNavigation)
        {
            if (ticket.IsBooked() || ticket.IsPending())
                continue;

            var seat = ev.SeatLayoutNavigation.SeatsNavigation.FirstOrDefault(s => s.Id == ticket.BookableSpaceId);
            if (seat == null)
                continue;

            var ticketComponent = PriceCalculationUtils.DecorateTicket(ev, seat);

            var newPrice = ticketComponent.GetPrice();

            if (ticket.Price != newPrice)
            {
                ticket.Price = newPrice;
                _dbContext.Tickets.Update(ticket);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ev;
    }
}

