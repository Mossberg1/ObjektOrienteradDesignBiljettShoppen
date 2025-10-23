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
    public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand, Booking>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IBookingTimer _bookingTimer;

        public PayBookingCommandHandler(IApplicationDbContext dbContext, IBookingTimer bookingTimer)
        {
            _dbContext = dbContext;
            _bookingTimer = bookingTimer;
        }

        public async Task<Booking> Handle(PayBookingCommand request, CancellationToken cancellationToken)
        {
         
            bool bookingExistsInTimer = _bookingTimer.RemoveBooking(request.Booking);
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

    public class PayBookingCommand : IRequest<Booking>
    {
        public Booking Booking { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    
        public PayBookingCommand(Booking booking, PaymentMethod paymentMethod)
        {
            Booking = booking;
            PaymentMethod = paymentMethod;
        }
    }

}