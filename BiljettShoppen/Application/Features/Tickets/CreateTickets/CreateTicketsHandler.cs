using Application.Decorators.TicketDecorator;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Tickets.CreateTickets;

public class CreateTicketsHandler : IRequestHandler<CreateTicketsCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateTicketsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> Handle(CreateTicketsCommand request, CancellationToken cancellationToken)
    {
        var ev = await _dbContext.Events
            .Include(e => e.SeatLayoutNavigation)
            .ThenInclude(sl => sl.SeatsNavigation)
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (ev is null)
            return false;

        var tickets = new List<Ticket>();

        // Skapa biljetter för varje plats i layouten.
        foreach (var seat in ev.SeatLayoutNavigation.SeatsNavigation)
        {
            ITicketComponent ticketComponent = new TicketComponent(ev);
            ticketComponent = new BookableSpaceDecorator(ticketComponent, seat);
            
            var ticket = new Ticket
            {
                Price = ticketComponent.GetPrice(),
                EventId = ev.Id,
                BookableSpaceId = seat.Id,
                Description = ticketComponent.GetDescription()
            };

            tickets.Add(ticket);
        }

        await _dbContext.Tickets.AddRangeAsync(tickets, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}