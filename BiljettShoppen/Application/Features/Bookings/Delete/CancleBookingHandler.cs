using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using DataAccess;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Features.Bookings.Delete;

namespace Application.Features.Bookings.Delete
/// <summary>
/// Hanterar borttagning av en <see cref="Models.Entities.Booking"/> från <see cref="IBookingTimer"/>.
/// <para>
/// Tar emot <see cref="DeleteBookingCommand"/> via MediatR och försöker ta bort bokningen med hjälp av referensnummer.
/// </para>
/// </summary>
{
    public class CancleBookingHandler : IRequestHandler<CancleBookingCommand, bool>
    {
        private readonly IBookingTimer _bookingTimer;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CancleBookingHandler> _logger;

        public CancleBookingHandler(
            IBookingTimer bookingTimer, 
            ApplicationDbContext dbContext,
            ILogger<CancleBookingHandler> logger
        )
        {
            _bookingTimer = bookingTimer;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CancleBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = _bookingTimer.GetBooking(request.ReferenceNumber);
            if (booking == null)
            {
                _logger.LogWarning($"Ingen bokning hittades med referensnummer: {request.ReferenceNumber}");
                return false;
            }

            booking.TicketsNavigation.ForEach(t => 
            {
                t.PendingBookingReference = null;
                _dbContext.Tickets.Update(t);
            });

            await _dbContext.SaveChangesAsync();

            bool removed = _bookingTimer.RemoveBooking(booking);
            if (removed)
            {
                _logger.LogInformation($"Bokning {request.ReferenceNumber} togs bort");
            }

            return removed;
        }
    }
}

