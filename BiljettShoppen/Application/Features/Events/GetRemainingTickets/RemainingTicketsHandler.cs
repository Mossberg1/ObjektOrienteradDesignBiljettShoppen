using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Application.Features.Events.GetRemainingTickets;

namespace Application.Features.Events.GetRemainingTickets
/// <summary>
/// Hanterar logiken för att hämta information om kvarvarande biljetter för ett evenemang.
/// </summary>
/// <remarks>
/// Hämtar evenemanget från databasen, beräknar kapaciteten baserat på antalet seats i seatlayouten, <br/>
/// och räknar sedan antalet sålda biljetter. Resultatet returneras som <see cref="RemainingTickets"/>.
/// </remarks>
{
    public class RemainingTicketsHandler : IRequestHandler<GetRemainingTicketsQuery, RemainingTickets>
    {
        private readonly IApplicationDbContext _dbContext;

        public RemainingTicketsHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RemainingTickets> Handle(GetRemainingTicketsQuery request, CancellationToken cancellationToken)
        {
            var theEvent = await _dbContext.Events.AsNoTracking()
                .Include(e => e.SeatLayoutNavigation)
                    .ThenInclude(sl => sl.SeatsNavigation)
                .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

            if (theEvent == null)
            {
                throw new IdNotFoundException($"Event med Id {request.EventId} hittades inte.");
            }

            var totalCapacity = theEvent.SeatLayoutNavigation.SeatsNavigation.Count();

            var soldTickets = await _dbContext.Tickets.CountAsync(t => t.EventId == request.EventId && t.BookingId != null, cancellationToken);

            var remainingTickets = totalCapacity - soldTickets;

            return new RemainingTickets
            {
                EventId = theEvent.Id,
                EventName = theEvent.Name,
                TicketsRemaining = remainingTickets
            };
        }
    }
}
