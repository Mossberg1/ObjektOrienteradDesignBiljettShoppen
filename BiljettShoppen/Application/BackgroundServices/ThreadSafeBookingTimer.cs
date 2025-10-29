using Application.Hubs;
using Application.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.Entities.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.BackgroundServices
{
    public class ThreadSafeBookingTimer : BackgroundService, IBookingTimer
    {
        private readonly ConcurrentDictionary<string, Booking> _bookings;

        private readonly IHubContext<BookingHub> _hubContext;
        private readonly ILogger<ThreadSafeBookingTimer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private const int _removeIntervalInMinutes = 10;
        private const int _checkIntervalInSeconds = 2;

        public ThreadSafeBookingTimer(
            IHubContext<BookingHub> hubContext, 
            ILogger<ThreadSafeBookingTimer> logger,
            IServiceScopeFactory scopeFactory
        ) 
        {
            _bookings = new ConcurrentDictionary<string, Booking>();

            _hubContext = hubContext;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken) 
        {
            await ClearAllPendingBookingsFromDatabase(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            _logger.LogInformation("ThreadSafeBookingTimer har startat.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var bookingsSnapshot = _bookings.Values.ToArray();

                foreach (var booking in bookingsSnapshot)
                {
                    if (booking.CreatedAt.AddMinutes(_removeIntervalInMinutes) <= DateTime.UtcNow)
                    {
                        if (_bookings.TryRemove(booking.ReferenceNumber, out _))
                        {
                            _logger.LogInformation($"Boknings tid har gått ut för: {booking.ReferenceNumber}");

                            using var scope = _scopeFactory.CreateScope();
                            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                            foreach (var ticket in booking.TicketsNavigation)
                            {
                                ticket.PendingBookingReference = null;
                                dbContext.Tickets.Update(ticket);
                            }

                            await dbContext.SaveChangesAsync();

                            await _hubContext.Clients.Group(booking.ReferenceNumber)
                                .SendAsync("BookingExpired", booking.ReferenceNumber, stoppingToken);
                        }
                    }
                }  
                
                await Task.Delay(TimeSpan.FromSeconds(_checkIntervalInSeconds), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken) 
        {
            await ClearAllPendingBookingsFromDatabase(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public async void AddBooking(Booking booking)
        {
            _bookings.TryAdd(booking.ReferenceNumber, booking);
            _logger.LogInformation($"Bokning: {booking.ReferenceNumber} tillagd i timern.");
        }

        public Booking? GetBooking(string bookingReference)
        {
            return _bookings.TryGetValue(bookingReference, out var booking) ? booking : null;
        }

        public bool RemoveBooking(Booking booking)
        {
            var result = _bookings.TryRemove(booking.ReferenceNumber, out _);
            if (result)
            { 
                _logger.LogInformation($"Bokning: {booking.ReferenceNumber} borttagen från timern.");
            }

            return result;
        }

        private async Task ClearAllPendingBookingsFromDatabase(CancellationToken cancellationToken) 
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pending = await dbContext.Tickets
                .Where(t => t.PendingBookingReference != null)
                .ToListAsync(cancellationToken);

            if (pending.Any())
            {
                foreach (var ticket in pending)
                {
                    ticket.PendingBookingReference = null;
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
