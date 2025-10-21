using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            NumberOfSeatsToSell = request.NumberOfSeatsToSell,
            NumberOfLogesToSell = request.NumberOfLogesToSell,
            Price = request.Price,
            Cost = request.Cost,
            Type = request.Type,
            IsFamilyFriendly = request.IsFamilyFriendly,
            ArenaId = request.ArenaId,
            SeatLayoutId = request.SeatLayoutId,
        };

        var arenaExists = await _dbContext.Arenas.AnyAsync(a => a.Id == request.ArenaId, cancellationToken); // Kollar så att den valda ArenaId existerar innan ett nytt event skapas
        if (!arenaExists)
            throw new KeyNotFoundException($"Arena with ID {request.ArenaId} not found.");

        _dbContext.Events.Add(eventEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return eventEntity;
    }
}