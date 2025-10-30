using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateInvoicePath
{
    public record UpdateInvoicePathCommand(Booking Booking, string FilePath) : IRequest<bool>
    {
    }
}
