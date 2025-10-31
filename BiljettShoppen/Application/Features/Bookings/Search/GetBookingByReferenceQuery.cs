using MediatR;
using Models.Entities;

namespace Application.Features.Bookings.Search;
/// <summary>
/// Query för att hämta <see cref="Booking"/> baserat på referensnummer.
/// <para>
/// Skickas via MediatR till <see cref="GetBookingByReferenceHandler"/> som returnerar bokningen inklusive biljetter, event och betalningar.
/// </para>
/// </summary>
/// <param name="ReferenceNumber">Referensnumret för bokningen som ska hämtas (<see cref="Booking"/>).</param>

public record GetBookingByReferenceQuery(
    string ReferenceNumber
) : IRequest<Booking?>;

