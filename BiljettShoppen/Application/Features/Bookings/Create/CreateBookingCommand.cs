using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Bookings.Create
{
    public record CreateBookingCommand(
        decimal TotalPrice,
        List<Ticket> TicketsNavigation
    ) : IRequest<Models.Entities.Booking>;
}
