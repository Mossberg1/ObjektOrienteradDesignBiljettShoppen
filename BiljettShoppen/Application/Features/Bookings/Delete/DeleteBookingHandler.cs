using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
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
    public class DeleteBookingHandler : IRequestHandler<DeleteBookingCommand, bool>
    {
        private readonly IBookingTimer _bookingTimer;
        private readonly ILogger<DeleteBookingHandler> _logger;

        public DeleteBookingHandler(IBookingTimer bookingTimer, ILogger<DeleteBookingHandler> logger)
        {
            _bookingTimer = bookingTimer;
            _logger = logger;
        }

        public Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = _bookingTimer.GetBooking(request.ReferenceNumber);
            if (booking == null)
            {
                _logger.LogWarning($"Ingen bokning hittades med referensnummer: {request.ReferenceNumber}");
                return Task.FromResult(false);
            }

            bool removed = _bookingTimer.RemoveBooking(booking);
            if (removed)
            {
                _logger.LogInformation($"Bokning {request.ReferenceNumber} togs bort");
            }

            return Task.FromResult(removed);
        }
    }
}

