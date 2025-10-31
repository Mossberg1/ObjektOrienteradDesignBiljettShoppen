using MediatR;
using Models.Entities;

namespace Application.Features.Events.Update;
/// <summary>
/// Kommandot för att uppdatera ett befintligt evenemang.
/// </summary>
/// <remarks>
/// Skickas via MediatR till <see cref="UpdateEventHandler"/> <br/>
/// för att uppdatera ett evenemangs egenskaper som namn, datum, tid, pris och kostnad.
/// </remarks>
/// <param name="Id">ID för evenemanget som ska uppdateras.</param>
/// <param name="Name">Namnet för evenemanget.</param>
/// <param name="Date">Datumet för evenemanget.</param>
/// <param name="StartTime">Den nya starttiden för evenemanget.</param>
/// <param name="EndTime">Den nya sluttiden för evenemanget.</param>
/// <param name="ReleaseTicketsDate">Det nya datumet då biljetter släpps för försäljning.</param>
/// <param name="Price">Det nya priset för evenemanget.</param>
/// <param name="Cost">Den nya kostnaden för evenemanget.</param>
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