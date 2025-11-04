using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Enums;

namespace DataAccess.Utils;

public class DataSeeder
{
    private readonly IApplicationDbContext _dbContext;

    public DataSeeder(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        if (await _dbContext.Arenas.AnyAsync())
        {
            return;
        }

        var arenas = await SeedArenasAsync();
        var entrances = await SeedEntrancesAsync(arenas);
        var seatLayouts = await SeedSeatLayoutsAsync(arenas);
        var seats = await SeedSeatsAsync(seatLayouts, entrances);
        var events = await SeedEventsAsync(arenas, seatLayouts);
        await SeedTicketsAsync(events, seats);
        var bookings = await SeedBookingsAsync(events);
        await SeedPaymentsAsync(bookings);
    }

    private async Task<List<Arena>> SeedArenasAsync()
    {
        var arenas = new List<Arena>
        {
            new("Tegelbyggnaden", "Falunvägen 15, 791 70 Falun", true),
            new("Träbyggnaden", "Borlängevägen 10, 784 34 Borlänge", true)
        };

        await _dbContext.Arenas.AddRangeAsync(arenas);
        await _dbContext.SaveChangesAsync();
        return arenas;
    }

    private async Task<List<Entrance>> SeedEntrancesAsync(List<Arena> arenas)
    {
        var entrances = new List<Entrance>();
        foreach (var arena in arenas)
        {
            entrances.Add(new("Huvudentré", false, arena.Id));
            entrances.Add(new("Sidoentré", false, arena.Id));
            entrances.Add(new("VIP-entré", true, arena.Id));
        }

        await _dbContext.Entrances.AddRangeAsync(entrances);
        await _dbContext.SaveChangesAsync();
        return entrances;
    }

    private async Task<List<SeatLayout>> SeedSeatLayoutsAsync(List<Arena> arenas)
    {
        var seatLayouts = new List<SeatLayout>
        {
            new("Stols konfigurations med 50 stolar", 5, 10, arenas[0].Id),
            new("Stols konfiguration med 77 stolar", 7, 10, arenas[1].Id) 
        };

        await _dbContext.SeatLayouts.AddRangeAsync(seatLayouts);
        await _dbContext.SaveChangesAsync();
        return seatLayouts;
    }

    private async Task<List<Seat>> SeedSeatsAsync(List<SeatLayout> seatLayouts, List<Entrance> entrances)
    {
        var seats = new List<Seat>();
        foreach (var layout in seatLayouts)
        {
            var regularEntrance = entrances.First(e => e.ArenaId == layout.ArenaId && !e.VipEntrance);
            for (var row = 1; row <= layout.NumberOfRows; row++)
            {
                var isBenchRow = row == layout.NumberOfRows;
                for (var col = 1; col <= layout.NumberOfCols; col++)
                {
                    var type = isBenchRow ? SeatType.Bench : SeatType.Chair;
                    var price = isBenchRow ? 200 : 250;
                    seats.Add(new(row, col, type, layout.Id, price, regularEntrance.Id));
                }
            }
        }

        await _dbContext.Seats.AddRangeAsync(seats);
        await _dbContext.SaveChangesAsync();
        return seats;
    }

    private async Task<List<Event>> SeedEventsAsync(List<Arena> arenas, List<SeatLayout> seatLayouts)
    {
        var events = new List<Event>
        {
            new(
                "Konsert med lokala band",
                DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)),
                new TimeOnly(19, 0, 0),
                new TimeOnly(22, 0, 0),
                DateTime.UtcNow.AddDays(-1),
                300,
                5000,
                EventType.Concert, 
                true,
                arenas[0].Id,
                seatLayouts[0].Id
            ),
            new(
                "Stand-up kväll",
                DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                new TimeOnly(20, 0, 0),
                new TimeOnly(21, 30, 0),
                DateTime.UtcNow.AddDays(-1),
                450,
                8000,
                EventType.ComedyShow,
                false,
                arenas[1].Id,
                seatLayouts[1].Id
            )
        };

        var eventTypes = Enum.GetValues<EventType>();
        var extraCount = 128;

        for (var i = 1; i <= extraCount; i++)
        {
            var layout = seatLayouts[(i - 1) % seatLayouts.Count];
            var arenaId = layout.ArenaId;

            var daysUntilEvent = 14 + (i * 7);
            var eventDateTime = DateTime.UtcNow.AddDays(daysUntilEvent);
            var eventDate = DateOnly.FromDateTime(eventDateTime);

            DateTime releaseDate;
            if (i % 3 == 0)
            {
                releaseDate = DateTime.UtcNow.AddDays(-Math.Max(1, i % 7));
            }
            else
            {
                releaseDate = eventDateTime.AddDays(-Math.Max(2, (i % 5) + 1));
            }

            var start = new TimeOnly(19, 0);
            var end = new TimeOnly(22, 0);

            var type = eventTypes[(i - 1) % eventTypes.Length];

            var seatsToSell = layout.NumberOfRows * layout.NumberOfCols;
            var logesToSell = 0;

            var price = 150 + (i % 5) * 50;
            var cost = 500 + i * 10;

            var isFamilyFriendly = type == EventType.Theater || type == EventType.Concert;

            events.Add(new(
                $"Demo {type} #{i}",
                eventDate,
                start,
                end,
                releaseDate,
                price,
                cost,
                type,
                isFamilyFriendly,
                arenaId,
                layout.Id
            ));
        }

        await _dbContext.Events.AddRangeAsync(events);
        await _dbContext.SaveChangesAsync();
        return events;
    }

    private async Task SeedTicketsAsync(List<Event> events, List<Seat> seats)
    {
        var tickets = new List<Ticket>();
        foreach (var ev in events)
        {
            var eventSeats = seats.Where(s => s.SeatLayoutId == ev.SeatLayoutId);
            foreach (var seat in eventSeats)
            {
                var totalPrice = ev.Price + seat.Price;
                var description = $"Evenemang: {ev.Name} — {ev.Date.ToString("d MMM yyyy")} kl. {ev.StartTime.ToString("HH:mm")}, {seat.GetDescription()}, Pris: {totalPrice:0} kr";

                tickets.Add(new(totalPrice, description, ev.Id, seat.Id));
            }
        }

        await _dbContext.Tickets.AddRangeAsync(tickets);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<List<Booking>> SeedBookingsAsync(List<Event> events)
    {
        var bookings = new List<Booking>();

        var concert = events.FirstOrDefault(e => e.Type == EventType.Concert && e.ReleaseTicketsDate <= DateTime.UtcNow)
                      ?? events.FirstOrDefault(e => e.Type == EventType.Concert);

        if (concert == null)
            return bookings;

        var concertTickets = await _dbContext.Tickets
            .Include(t => t.BookableSpaceNavigation)
            .Where(t => t.EventId == concert.Id && t.BookingId == null)
            .OrderBy(t => t.Id)
            .Take(2)
            .ToListAsync();

        if (concertTickets.Count == 2)
        {
            var totalPrice = concertTickets.Sum(t => t.Price);
            var booking1 = new Booking("test@example.com", totalPrice, true); 

            await _dbContext.Bookings.AddAsync(booking1);
            await _dbContext.SaveChangesAsync();

            for (var i = 0; i < concertTickets.Count; i++)
            {
                var ticket = concertTickets[i];
                ticket.BookingId = booking1.Id;
                _dbContext.Tickets.Update(ticket);
            }

            await _dbContext.SaveChangesAsync();

            bookings.Add(booking1);
        }

        return bookings;
    }

    private async Task SeedPaymentsAsync(List<Booking> bookings)
    {
        var paidBookings = bookings.Where(b => b.IsPaid);
        foreach (var booking in paidBookings)
        {
            var payment = new Payment(booking.Id, PaymentMethod.CreditCard);
            await _dbContext.Payments.AddAsync(payment);
        }
        await _dbContext.SaveChangesAsync();
    }
}