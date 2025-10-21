using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    int NumberOfSeatsToSell,
    int NumberOfLogesToSell,
    decimal Price,
    decimal Cost,
    EventType Type,
    bool IsFamilyFriendly,
    int ArenaId,
    int SeatLayoutId
    ) : IRequest<Event>;

