using MediatR;
using Models.Entities;

namespace Application.Features.Bookings.Create
{
    public record CreateBookingCommand(
        decimal TotalPrice,
        List<Ticket> TicketsNavigation
    ) : IRequest<Models.Entities.Booking>;
}
