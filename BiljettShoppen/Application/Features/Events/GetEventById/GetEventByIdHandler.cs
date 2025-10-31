using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.GetById;
/// <summary>
/// Hanterar <see cref="GetEventByIdQuery"/> och ansvarar för att hämta ett <see cref="Event"/> baserat på Id.
/// </summary>
/// <remarks>
/// <para>
/// Hämtar eventet från databasen samt <see cref="ArenaNavigation"/> och <see cref="SeatLayoutNavigation"/>.
/// Resultatet spåras inte i Entity Framework (.AsNoTracking()).
/// </para>
/// </remarks>
public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, Event?>
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