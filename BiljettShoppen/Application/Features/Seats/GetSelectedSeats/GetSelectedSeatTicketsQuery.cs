using MediatR;
using Models.Entities;

namespace Application.Features.Seats.GetSelectedSeats;
/// <summary>
/// En query för att hämta biljetter för valda sittplatser i ett evenemang.
/// <para>
/// Skickas via MediatR till <see cref="GetSelectedSeatTicketsHandler"/> som returnerar en lista 
/// av <see cref="Ticket"/> som motsvarar valda platser.
/// </para>
/// </summary>
/// <param name="SelectedSeatsIds">ID:n för de valda sittplatserna.</param>
/// <param name="EventId">ID för evenemanget som biljetterna tillhör.</param>

public record GetSelectedSeatTicketsQuery(int[] SelectedSeatsIds, int EventId) : IRequest<List<Ticket>>;