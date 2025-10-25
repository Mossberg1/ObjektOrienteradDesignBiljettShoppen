using DataAccess.Interfaces;
using DataAccess.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.Browse;

public class BrowseEventsHandler : IRequestHandler<BrowseEventsQuery, PaginatedList<Event>>
{
    private readonly IApplicationDbContext _dbContext;

    public BrowseEventsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PaginatedList<Event>> Handle(BrowseEventsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Events
            .Include(e => e.ArenaNavigation)
            .Include(e => e.TicketsNavigation)
            .Where(e => e.ReleaseTicketsDate <= DateTime.UtcNow)
            .AsNoTracking();
        
        if (request.ToDate.HasValue)
        {
            queryable = queryable.Where(e => e.Date <= request.ToDate.Value);
        }

        if (request.FromDate.HasValue)
        {
            queryable = queryable.Where(e => e.Date >= request.FromDate.Value);
        }

        if (request.Type.HasValue)
        {
            queryable = queryable.Where(e => e.Type == request.Type.Value);
        }

        if (request.IsFamilyFriendly.HasValue)
        {
            queryable = queryable.Where(e => e.IsFamilyFriendly == request.IsFamilyFriendly.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchWord))
        {
            queryable = queryable.Where(e => 
                EF.Functions.ILike(e.Name, $"%{request.SearchWord}%") ||
                EF.Functions.ILike(e.ArenaNavigation.Name, $"%{request.SearchWord}%") ||
                EF.Functions.ILike(e.ArenaNavigation.Address, $"%{request.SearchWord}%")
            );
        }

        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case ("name"):
                { 
                    queryable = request.Ascending 
                        ? queryable.OrderBy(e => e.Name) 
                        : queryable.OrderByDescending(e => e.Name);
                    break;
                }
                case ("startdate"):
                {
                    queryable = request.Ascending 
                        ? queryable.OrderBy(e => e.Date).ThenBy(e => e.StartTime)
                        : queryable.OrderByDescending(e => e.Date).ThenBy(e => e.StartTime);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        return await PaginatedList<Event>.CreateAsync(queryable, request.PageNumber, request.PageSize, cancellationToken);
    }
}