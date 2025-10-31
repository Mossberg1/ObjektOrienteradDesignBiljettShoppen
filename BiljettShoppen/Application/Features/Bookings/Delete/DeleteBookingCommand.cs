using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bookings.Delete
/// <summary>
/// Kommandot används för att ta bort en befintlig <see cref="Models.Entities.Booking"/>.
/// <para>
/// Skickas via MediatR till DeleteBookingHandler som tar bort bokningen från databasen med hjälp av referensnummer.
/// <para>
/// </summary>
/// <param name="ReferenceNumber">Referensnumret för bokningen som ska tas bort (<see cref="Models.Entities.Booking"/>).</param>

{
    public record DeleteBookingCommand(string ReferenceNumber) : IRequest<bool>;
}
