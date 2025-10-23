using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Booking.Create
{
    public record CreateBookingCommand(
        decimal TotalPrice,
        bool IsPaid,
        Payment? PaymentNavigation,
        List<Ticket> TicketsNavigation
    ) : IRequest<Models.Entities.Booking>;
}
