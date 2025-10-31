using MediatR;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Events.Create;
/// <summary>
/// Kommando för att skapa ett nytt <see cref="Event"/>.
/// <para>
/// Innehåller alla parametrar för att skapa ett event, inklusive datum, tid, pris, arena och seatlayout.
/// </para>
/// </summary>
/// <param name="Name">Namnet på eventet.</param>
/// <param name="Date">Datum för eventet.</param>
/// <param name="StartTime">Starttid för eventet.</param>
/// <param name="EndTime">Sluttid för eventet.</param>
/// <param name="ReleaseTicketsDate">Datum då biljetterna släpps.</param>
/// <param name="Price">Pris per biljett.</param>
/// <param name="Cost">Kostnad för eventet.</param>
/// <param name="Type">Typ av event (<see cref="EventType"/>).</param>
/// <param name="IsFamilyFriendly">Anger om eventet är familjevänligt (true) eller inte (false).</param>
/// <param name="ArenaId">Unikt ID för en specifik arenan där eventet hålls.</param>
/// <param name="SeatLayoutId">ID för seatlayouten som används för eventet.</param>
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