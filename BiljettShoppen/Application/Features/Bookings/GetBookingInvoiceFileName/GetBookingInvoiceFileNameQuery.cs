using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.GetBookingInvoiceFileName
{
    public record GetBookingInvoiceFileNameQuery(string ReferenceNumber) : IRequest<string?>
    {
    }
}
