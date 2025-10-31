using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Events.Delete;
/// <summary>
/// Hanterar <see cref="DeleteEventCommand"/> och ansvarar för att ta bort ett <see cref="Event"/> från databasen.
/// </summary>
/// <remarks>
/// <para>
/// Hämtar först eventet baserat på <see cref="DeleteEventCommand.EventId"/>. <br/>
/// Om eventet hittas tas det bort från databasen.
/// </para>
/// <para>
/// Returnerar true om en eller flera rader påverkades (borttagningen lyckades), annars false.
/// </para>
/// </remarks>
public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = await _dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

        if (eventEntity == null)
            return false;

        _dbContext.Events.Remove(eventEntity);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}