using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateConfirmationPath
{
    public record UpdateConfirmationPathCommand(Booking Booking, string PdfPath) : IRequest<bool>
    {
    }
}
