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
            var booking = new Models.Entities.Booking
            {
                TotalPrice = request.TotalPrice,
                IsPaid = request.IsPaid,
                PaymentNavigation = request.PaymentNavigation,
                TicketsNavigation = request.TicketsNavigation,
            };

            _bookingTimer.AddBooking(booking);

            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return booking;
        }
    }
}
