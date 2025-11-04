using Application.Decorators.TicketDecorator;
using Application.Utils;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Tickets.CreateTickets;
/// <summary>
/// Hanterar skapandet av biljetter för ett specifikt evenemang.
/// <para>
/// Tar emot <see cref="CreateTicketsCommand"/> via MediatR och genererar biljetter <br/>
/// baserat på evenemangets seat-layout.
/// </para>
/// </summary>
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
            var ticketComponent = PriceCalculationUtils.DecorateTicket(ev, seat);
            var price = ticketComponent.GetPrice();
            var description = ticketComponent.GetDescription();

            var ticket = new Ticket(price, description, ev.Id, seat.Id);

            tickets.Add(ticket);
        }

        await _dbContext.Tickets.AddRangeAsync(tickets, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}