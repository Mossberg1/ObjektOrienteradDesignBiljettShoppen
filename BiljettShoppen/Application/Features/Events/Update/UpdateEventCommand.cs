using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.Update;

public record UpdateEventCommand(
    int Id,
    string Name,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    DateTime ReleaseTicketsDate,
    decimal Price,
    decimal Cost
) : IRequest<Event?>;