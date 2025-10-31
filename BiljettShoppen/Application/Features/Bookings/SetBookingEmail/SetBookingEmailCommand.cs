using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.SetBookingEmail
/// <summary>
/// Kommandot används för att uppdatera e-postadressen för en befintlig <see cref="Booking"/>.
/// <para>
/// Skickas via MediatR till SetBookingEmailHandler som uppdaterar bokningens e-post i databasen.
/// </para>
/// </summary>
/// <param name="Booking">Bokningen som ska uppdateras (<see cref="Booking"/>).</param>
/// <param name="Email">Den nya e-postadressen som ska sättas för bokningen.</param>
{
    public record SetBookingEmailCommand(Booking Booking, string Email) : IRequest<bool>
    {
    }
}
