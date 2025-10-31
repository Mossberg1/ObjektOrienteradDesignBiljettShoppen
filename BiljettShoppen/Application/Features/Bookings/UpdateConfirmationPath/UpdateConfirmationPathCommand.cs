using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateConfirmationPath
/// <summary>
/// Används för att uppdatera sökvägen till bekräftelse-PDF för en befintlig <see cref="Booking"/>.
/// <para>
/// Skickas via MediatR till UpdateConfirmationPathHandler som uppdaterar bokningens ConfirmationPdfPath i databasen.
/// </para>
/// </summary>
/// <param name="Booking">Bokningen som ska uppdateras (<see cref="Booking"/>).</param>
/// <param name="PdfPath">Den nya sökvägen till PDF-filen som sätts.</param>
{
    public record UpdateConfirmationPathCommand(Booking Booking, string PdfPath) : IRequest<bool>
    {
    }
}
