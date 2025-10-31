using MediatR;
using Models.Entities;

namespace Application.Features.Events.ViewSeats;
/// <summary>
/// Query som används för att hämta evenemang med seatlayout och arena.
/// </summary>
/// <remarks>
/// Returnerar ett <see cref="Event"/>-objekt med sorterade platser om evenemanget finns.
/// Returnerar null om evenemanget med det angivna Id:t inte finns.
/// </remarks>
/// <param name="EventId">Id för evenemanget som ska hämtas.</param>
public record ViewSeatsQuery(int EventId) : IRequest<Event?>;