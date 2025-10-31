using MediatR;
using Models.Entities;

namespace Application.Features.Bookings.Create
/// <summary>
/// Kommandot används för att skapa en ny <see cref="Booking"/>.
/// <para>
/// Skickas via MediatR till en handler som skapar bokningen i databasen.
/// </para>
/// </summary>
/// <param name="TotalPrice">Det totala priset för bokningen.</param>
/// <param name="TicketsNavigation">Lista med <see cref="Ticket"/>-objekt som ingår i bokningen.</param>
{
    public record CreateBookingCommand(
        decimal TotalPrice,
        List<Ticket> TicketsNavigation
    ) : IRequest<Models.Entities.Booking>;
}
