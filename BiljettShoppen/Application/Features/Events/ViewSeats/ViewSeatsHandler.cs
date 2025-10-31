using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.ViewSeats;
/// <summary>
/// Handler som tar emot <see cref="ViewSeatsQuery"/> via MediatR <br/>
/// och hämtar ett evenemang med platser sorterade per rad och kolumn.
/// </summary>
/// <remarks>
/// Hämtar information om arenan och dess seatlayout.
/// Returnerar null om evenemanget med det angivna Id:t inte finns.
/// </remarks>
/// <param name="request">Kommandot som innehåller Id för evenemanget som ska visas.</param>
/// <param name="cancellationToken">Token för att avbryta operationen.</param>
/// <returns>Evenemanget med dess seatlayout och arena, eller null om det inte finns.</returns>
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