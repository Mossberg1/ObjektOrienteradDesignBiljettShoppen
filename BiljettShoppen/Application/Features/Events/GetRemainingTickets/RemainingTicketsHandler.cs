using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Events.GetRemainingTickets
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
                throw new Exception($"Event with Id {request.EventId} not found.");
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
