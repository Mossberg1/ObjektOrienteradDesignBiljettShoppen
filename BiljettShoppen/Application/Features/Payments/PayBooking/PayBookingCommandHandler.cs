using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Enums;

namespace Application.Features.Payments.PayBooking
{
    public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand, Models.Entities.Booking>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBookingTimer _bookingTimer;

        public PayBookingCommandHandler(IApplicationDbContext dbContext, IBookingTimer bookingTimer)
        {
            _dbContext = dbContext;
            _bookingTimer = bookingTimer;
        }

        public async Task<Models.Entities.Booking> Handle(PayBookingCommand request, CancellationToken cancellationToken)
        {
         
            bool bookingExistsInTimer = _bookingTimer.RemoveBooking(request.Booking.ReferenceNumber);
            if (!bookingExistsInTimer)
            {
                throw new Exception("Booking not found in timer.");
            }

            request.Booking.IsPaid = true;
            await _dbContext.Bookings.AddAsync(request.Booking, cancellationToken);

            var payment = new Payment
            {
                PaymentMethod = request.PaymentMethod,
                BookingNavigation = request.Booking
            };

            await _dbContext.Payments.AddAsync(payment);
       
            await _dbContext.SaveChangesAsync(cancellationToken);

            return request.Booking!;
        }
    }

    public class PayBookingCommand : IRequest<Models.Entities.Booking>
    {
        public Models.Entities.Booking Booking { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    
        public PayBookingCommand(Models.Entities.Booking booking, PaymentMethod paymentMethod)
        {
            Booking = booking;
            PaymentMethod = paymentMethod;
        }
    }

}