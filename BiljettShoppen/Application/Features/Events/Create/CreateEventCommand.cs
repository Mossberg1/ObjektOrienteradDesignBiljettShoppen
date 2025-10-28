using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.Create;


public record CreateEventCommand(
    string Name,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    DateTime ReleaseTicketsDate,
    decimal Price,
    decimal Cost,
    EventType Type,
    bool IsFamilyFriendly,
    int ArenaId,
    int SeatLayoutId
) : IRequest<Event>;