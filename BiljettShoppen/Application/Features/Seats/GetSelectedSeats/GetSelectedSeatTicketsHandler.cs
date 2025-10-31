using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Seats.GetSelectedSeats;
/// <summary>
/// Hämtar biljetter för specifikt valda sittplatser i ett evenemang.
/// <para>
/// Tar emot <see cref="GetSelectedSeatTicketsQuery"/> via MediatR och returnerar <br/>
/// en lista av <see cref="Ticket"/> som matchar valda platser.
/// </para>
/// </summary>

public class GetSelectedSeatTicketsHandler : IRequestHandler<GetSelectedSeatTicketsQuery, List<Ticket>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetSelectedSeatTicketsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Ticket>> Handle(GetSelectedSeatTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _dbContext.Tickets
            .Where(t => request.SelectedSeatsIds.Contains(t.BookableSpaceId) && t.EventId == request.EventId)
            .Include(t => t.BookableSpaceNavigation)
            .ToListAsync(cancellationToken);

        return tickets;
    }
}