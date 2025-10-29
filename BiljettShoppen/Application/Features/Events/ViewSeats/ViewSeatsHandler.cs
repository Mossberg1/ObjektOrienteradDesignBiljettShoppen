using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.ViewSeats;

public class ViewSeatsHandler : IRequestHandler<ViewSeatsQuery, Event?>
{
    private readonly IApplicationDbContext _dbContext;

    public ViewSeatsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> Handle(ViewSeatsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Events
            .Include(e => e.SeatLayoutNavigation)
            .ThenInclude(sl => sl.SeatsNavigation
                .OrderBy(s => s.RowNumber)
                .ThenBy(s => s.ColNumber)
            )
            .Include(e => e.ArenaNavigation)
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken: cancellationToken);
    }
}