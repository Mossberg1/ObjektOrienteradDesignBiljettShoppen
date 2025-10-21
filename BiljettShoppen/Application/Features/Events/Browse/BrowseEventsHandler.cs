using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.Browse;

public class BrowseEventsHandler : IRequestHandler<BrowseEventsQuery, List<Event>>
{
    private readonly IApplicationDbContext _dbContext;

    public BrowseEventsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Event>> Handle(BrowseEventsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Events.Include(e => e.ArenaNavigation).ToListAsync(cancellationToken);
    }
}