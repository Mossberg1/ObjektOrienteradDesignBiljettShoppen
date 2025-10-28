using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.GetById;

public class GetEventByIdHandler:IRequestHandler<GetEventByIdQuery, Event?>
{
    private readonly IApplicationDbContext _dbContext;

    public GetEventByIdHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Events
            .Include(e => e.ArenaNavigation)
            .Include(e => e.SeatLayoutNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
    }
}