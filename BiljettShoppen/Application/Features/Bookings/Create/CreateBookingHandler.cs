using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Exceptions;

namespace Application.Features.Booking.Create
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, Models.Entities.Booking>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBookingTimer _bookingTimer;

        public CreateBookingHandler(IApplicationDbContext dbContext, IBookingTimer bookingTimer)
        {
            _dbContext = dbContext;
            _bookingTimer = bookingTimer;
        }

        public async Task<Models.Entities.Booking> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
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
            
            var booking = new Models.Entities.Booking
            {
                TotalPrice = request.TotalPrice,
                TicketsNavigation = request.TicketsNavigation,
            };

            _bookingTimer.AddBooking(booking);

            //await _dbContext.Bookings.AddAsync(booking, cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);

            return booking;
        }
    }
}
