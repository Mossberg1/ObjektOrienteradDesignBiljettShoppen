using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Seats.GetSelectedSeats;

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