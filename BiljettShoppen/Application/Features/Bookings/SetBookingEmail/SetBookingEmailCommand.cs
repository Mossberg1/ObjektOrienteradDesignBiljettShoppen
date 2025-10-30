using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.SetBookingEmail
{
    public record SetBookingEmailCommand(Booking Booking, string Email) : IRequest<bool>
    {
    }
}
