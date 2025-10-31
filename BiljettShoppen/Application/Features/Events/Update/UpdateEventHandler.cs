using Application.Utils;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.Update;
/// <summary>
/// Handler som tar emot <see cref="UpdateEventCommand"/> via MediatR <br/>
/// och uppdaterar ett befintligt evenemang i databasen.
/// </summary>
/// <remarks>
/// Om evenemanget med ID:t inte finns så returneras null.<br/>
/// Efter uppdateringen så sparas ändringarna i databasen.
/// </remarks>
/// <param name="request">Kommandot som innehåller de nya värdena för evenemanget.</param>
/// <param name="cancellationToken">En token för att avbryta operationen.</param>
/// <returns>Det uppdaterade <see cref="Event"/>-objektet, eller null om evenemanget inte finns eller ändringarna inte kunde sparas.</returns>
public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Event?>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event?> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (eventEntity == null)
            return null;

        eventEntity.Name = request.Name;
        eventEntity.Date = request.Date;
        eventEntity.StartTime = request.StartTime;
        eventEntity.EndTime = request.EndTime;
        eventEntity.ReleaseTicketsDate = DateTimeUtils.ToUtc(request.ReleaseTicketsDate);
        eventEntity.Price = request.Price;
        eventEntity.Cost = request.Cost;

        if (await _dbContext.SaveChangesAsync(cancellationToken) == 0)
            return null;

        return eventEntity;
    }
}