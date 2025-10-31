using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateInvoicePath
/// <summary>
/// Kommandot används för att uppdatera fakturans filväg för en befintlig <see cref="Booking"/>.
/// <para>
/// Skickas via MediatR till UpdateInvoicePathHandler som uppdaterar bokningens InvoiceFilePath i databasen.
/// </para>
/// </summary>
/// <param name="Booking">Bokningen som ska uppdateras (<see cref="Booking"/>).</param>
/// <param name="FilePath">Den nya filvägen till fakturan som ska sättas.</param>
{
    public record UpdateInvoicePathCommand(Booking Booking, string FilePath) : IRequest<bool>
    {
    }
}
