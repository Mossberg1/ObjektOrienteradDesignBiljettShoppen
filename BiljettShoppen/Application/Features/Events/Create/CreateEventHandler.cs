using Application.Features.Tickets.CreateTickets;
using Application.Utils;
using DataAccess.Interfaces;
using MediatR;
using Models.Entities;

namespace Application.Features.Events.Create;
/// <summary>
/// Handler som tar emot <see cref="CreateEventCommand"/> och skapar ett nytt <see cref="Event"/> i databasen.
/// <para>
/// Hanterar instansiering av <see cref="Event"/> med parametrar från CreateEventCommand, konverterar ReleaseTicketsDate till UTC tidszon och sparar sedan eventet.
/// </para>
/// </summary>
public class CreateEventHandler : IRequestHandler<CreateEventCommand, Event>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public CreateEventHandler(IApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Event> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventEntity = new Event(
            request.Name,
            request.Date,
            request.StartTime,
            request.EndTime,
            request.ReleaseTicketsDate,
            request.Price,
            request.Cost,
            request.Type,
            request.IsFamilyFriendly,
            request.ArenaId,
            request.SeatLayoutId
        );

        eventEntity.ReleaseTicketsDate = DateTimeUtils.ToUtc(request.ReleaseTicketsDate);

        await _dbContext.Events.AddAsync(eventEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var createTicketsCommand = new CreateTicketsCommand(eventEntity.Id);
        await _mediator.Send(createTicketsCommand, cancellationToken);

        return eventEntity;
    }
}