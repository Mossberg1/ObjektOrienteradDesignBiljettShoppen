using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Entities;

namespace Application.BackgroundServices;

// Använd ConcurrentDictionary istället för thread-safety.
// Kolla istället alla bokningar i listan varje iteration.
// För tillfället är operationen o(n) även fast vi kan hoppa ur tidigare.
// Däremot eftersom listan inte har thread-safety så kan vi inte garantera att bokningarna ligger i ordning.
// Varje iteration kommer fortfarande vara o(n) även fast vi kollar igenom alla bokningar.
// Med denna implementation är även borttagning o(n) och hitta bokning o(n).
// Med ConcurrentDictionary blir borttagning o(1) och hitta bokning o(1).'
// En ConcurrentDictionary undviker att elements behöver flyttas i minnet vid borttagning?
// En ConcurrentDictionary behöver inte skapa en ny array när max capacity nås?
public class BookingTimer : BackgroundService, IBookingTimer
{
    private readonly ILogger<BookingTimer> _logger; 
    private List<Booking> _bookings;

    private const int _removeIntervalInMinutes = 10;
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
                    _logger.LogInformation($"Boknings tid har gått ut: {firstBooking.ReferenceNumber}");

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
        _logger.LogInformation($"Bokning tillagd i timern: {booking.ReferenceNumber}");
        _bookings.Add(booking);
    }
    
    public bool RemoveBooking(Booking booking)
    {
        _logger.LogInformation($"Bokning borttagen från timern: {booking.ReferenceNumber}");
        return _bookings.Remove(booking);
    }

    public Booking? GetBooking(string bookingReference)
    {
        return _bookings.FirstOrDefault(b => b.ReferenceNumber == bookingReference);
    }
}