using Application.Interfaces;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Application.Features.Bookings.Create;
using Models.Entities;

namespace Application.Features.Bookings.Create
/// <summary>
/// Hanterar skapandet av en ny <see cref="Models.Entities.Booking"/>.
/// <para>
/// Tar emot <see cref="CreateBookingCommand"/> via MediatR, validerar biljetter <br/> 
/// och skapar bokningen. Lägger den även i <see cref="IBookingTimer"/> och sparar i databasen.
/// </para>
/// </summary>
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Booking>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBookingTimer _bookingTimer;

        public CreateBookingHandler(IApplicationDbContext dbContext, IBookingTimer bookingTimer)
        {
            _dbContext = dbContext;
            _bookingTimer = bookingTimer;
        }

        public async Task<Booking> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var eventId = request.TicketsNavigation.FirstOrDefault()?.EventId;
            if (eventId.HasValue)
            {
                var ev = await _dbContext.Events
                    .FirstOrDefaultAsync(e => e.Id == eventId.Value, cancellationToken);

                int ticketCount = request.TicketsNavigation.Count;

                if (!ev.IsFamilyFriendly && ticketCount > 5)
                {
                    throw new BookingLimitExceededException("Du kan bara boka max 5 biljetter för detta evenemang.");
                }
            }

            var booking = new Booking(request.TotalPrice, request.TicketsNavigation);

             _bookingTimer.AddBooking(booking);

            foreach (var ticket in booking.TicketsNavigation)
            {
                ticket.PendingBookingReference = booking.ReferenceNumber;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return booking;
        }
    }
}
