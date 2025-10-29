using Application.Utils;
using DataAccess.Interfaces;
using MediatR;
using Models.Entities;

namespace Application.Features.Events.Create;

public class CreateEventHandler : IRequestHandler<CreateEventCommand, Event>
{
    private readonly IApplicationDbContext _dbContext;
    public CreateEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = new Event
        {
            Name = request.Name,
            Date = request.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ReleaseTicketsDate = request.ReleaseTicketsDate,
            Price = request.Price,
            Cost = request.Cost,
            Type = request.Type,
            IsFamilyFriendly = request.IsFamilyFriendly,
            ArenaId = request.ArenaId,
            SeatLayoutId = request.SeatLayoutId,
        };

        eventEntity.ReleaseTicketsDate = DateTimeUtils.ToUtc(request.ReleaseTicketsDate);

        _dbContext.Events.Add(eventEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return eventEntity;
    }
}