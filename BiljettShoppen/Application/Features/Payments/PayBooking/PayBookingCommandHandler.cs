using Application.Interfaces;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Application.Features.Payments.PayBooking;

namespace Application.Features.Payments.PayBooking
/// <summary>
/// Hanterar betalningen av en bokning.
/// <para>
/// Tar emot <see cref="PayBookingCommand"/> via MediatR, markerar bokningen som betald, <br/>
/// uppdaterar biljetter och registrerar betalningen i databasen.
/// </para>
/// </summary>
{
    public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand, Models.Entities.Booking>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBookingTimer _bookingTimer;
        private readonly ILogger<PayBookingCommandHandler> _logger;

        public PayBookingCommandHandler(ApplicationDbContext dbContext, IBookingTimer bookingTimer, ILogger<PayBookingCommandHandler> logger)
        {
            _dbContext = dbContext;
            _bookingTimer = bookingTimer;
            _logger = logger;
        }

        public async Task<Models.Entities.Booking> Handle(PayBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = _bookingTimer.GetBooking(request.BookingReferenceNumber);
            if (booking == null)
            {
                throw new ReferenceNumberNotFoundException("Bokningen hittades inte.");
            }

            if (!_bookingTimer.RemoveBooking(booking))
            {
                throw new ReferenceNumberNotFoundException("Bokningen hittades inte i timern.");
            }

            booking.IsPaid = true;

            await _dbContext.Bookings.AddAsync(booking, cancellationToken);

            foreach (var ticket in booking.TicketsNavigation)
            {
                _dbContext.Entry(ticket.BookableSpaceNavigation).State = EntityState.Unchanged;
                _dbContext.Entry(ticket.EventNavigation).State = EntityState.Unchanged;

                ticket.BookingNavigation = booking;
                ticket.PendingBookingReference = null;

                _dbContext.Tickets.Update(ticket);
            }

            var payment = new Payment(booking, request.PaymentMethod);

            await _dbContext.Payments.AddAsync(payment, cancellationToken);

            if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
            {
                _logger.LogInformation($"Bokning betald: {booking.ReferenceNumber}");
            }

            return booking;
        }
    }

    public class PayBookingCommand : IRequest<Models.Entities.Booking>
    {
        public string BookingReferenceNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public PayBookingCommand(string bookingReferenceNumber, PaymentMethod paymentMethod)
        {
            BookingReferenceNumber = bookingReferenceNumber;
            PaymentMethod = paymentMethod;
        }
    }

}