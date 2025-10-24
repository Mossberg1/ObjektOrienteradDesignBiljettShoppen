using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Entities;

namespace Application.BackgroundServices;

// TODO: Optimera med linkedlist?
public class BookingTimer : BackgroundService, IBookingTimer
{
    private readonly ILogger<BookingTimer> _logger; 
    private List<Booking> _bookings;

    private const float _removeIntervalInMinutes = 0.1f;
    private const int _checkIntervalInSeconds = 10;

    public BookingTimer(ILogger<BookingTimer> logger)
    {
        _logger = logger;
        _bookings = [];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Boknings timern har startat.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_bookings.Count != 0)
            {
                var firstBooking = _bookings[0];
                while (firstBooking.CreatedAt.AddMinutes(_removeIntervalInMinutes) <= DateTime.UtcNow)
                {
                    _bookings.RemoveAt(0);
                    _logger.LogInformation($"Tog bort bokning med pris: {firstBooking.TotalPrice}");

                    if (_bookings.Count != 0)
                    {
                        firstBooking = _bookings[0];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(_checkIntervalInSeconds), stoppingToken);
        }
        
        _logger.LogInformation("Boknings timern har stoppats.");
    }

    public void AddBooking(Booking booking)
    {
        _bookings.Add(booking);
    }
    
    public bool RemoveBooking(string bookingReference)
    {
        var booking = GetBooking(bookingReference);
        return booking != null && _bookings.Remove(booking);
    }

    public Booking? GetBooking(string bookingReference)
    {
        return _bookings.FirstOrDefault(b => b.ReferenceNumber == bookingReference);
    }
}