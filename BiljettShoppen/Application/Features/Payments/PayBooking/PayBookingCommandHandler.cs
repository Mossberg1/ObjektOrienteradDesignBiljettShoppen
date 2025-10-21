using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public PayBookingCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Booking> Handle(PayBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            if (booking.IsPaid)
            {
                throw new Exception("Booking is already paid.");
            }

            var payment = new Payment
            {
                PaymentMethod = request.PaymentMethod,
                BookingId = booking.Id,
            };

            _dbContext.Payments.Add(payment);

            booking.IsPaid = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return booking!;
        }
    }

    public class PayBookingCommand : IRequest<Booking>
    {
        public int BookingId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}