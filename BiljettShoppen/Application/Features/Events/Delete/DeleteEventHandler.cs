using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Events.Delete;

public class DeleteEventHandler:IRequestHandler<DeleteEventCommand, bool>
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