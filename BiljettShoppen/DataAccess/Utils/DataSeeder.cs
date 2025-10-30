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
            new()
            {
                Address = "Falunvägen 15, 791 70 Falun", Name = "Tegelbyggnaden", NumberOfSeats = 50,
                NumberOfLoges = 4, Indoors = true
            },
            new()
            {
                Address = "Borlängevägen 10, 784 34 Borlänge", Name = "Träbyggnaden", NumberOfSeats = 77,
                NumberOfLoges = 0, Indoors = true
            }
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
            entrances.Add(new Entrance { Name = "Huvudentré", VipEntrance = false, ArenaId = arena.Id });
            entrances.Add(new Entrance { Name = "Sidoentré", VipEntrance = false, ArenaId = arena.Id });
            if (arena.NumberOfLoges > 0)
            {
                entrances.Add(new Entrance { Name = "VIP-entré", VipEntrance = true, ArenaId = arena.Id });
            }
        }

        await _dbContext.Entrances.AddRangeAsync(entrances);
        await _dbContext.SaveChangesAsync();
        return entrances;
    }

    private async Task<List<SeatLayout>> SeedSeatLayoutsAsync(List<Arena> arenas)
    {
        var seatLayouts = new List<SeatLayout>
        {
            new() { Name = "Stols konfigurations med 50 stolar", NumberOfRows = 5, NumberOfCols = 10, ArenaId = arenas[0].Id }, // 50 seats
            new() { Name = "Stols konfiguration med 77 stolar", NumberOfRows = 7, NumberOfCols = 11, ArenaId = arenas[1].Id }, // 77 seats
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
                    seats.Add(new Seat
                    {
                        RowNumber = row,
                        ColNumber = col,
                        Type = isBenchRow ? SeatType.Bench : SeatType.Chair,
                        Price = isBenchRow ? 200 : 250,
                        SeatLayoutId = layout.Id,
                        EntranceId = regularEntrance.Id
                    });
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
            new()
            {
                Name = "Konsert med lokala band", Date = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(2)),
                StartTime = new TimeOnly(19, 0, 0), EndTime = new TimeOnly(22, 0, 0),
                Price = 300, Cost = 5000, Type = EventType.Concert, IsFamilyFriendly = true, ArenaId = arenas[0].Id,
                SeatLayoutId = seatLayouts[0].Id
            },
            new()
            {
                Name = "Stand-up kväll", Date = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                StartTime = new TimeOnly(20, 0, 0), EndTime = new TimeOnly(21, 30, 0),
                Price = 450, Cost = 8000, Type = EventType.ComedyShow, IsFamilyFriendly = false, ArenaId = arenas[1].Id,
                SeatLayoutId = seatLayouts[1].Id
            }
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

            events.Add(new Event
            {
                Name = $"Demo {type} #{i}",
                Date = eventDate,
                StartTime = start,
                EndTime = end,
                ReleaseTicketsDate = releaseDate,
                Price = price,
                Cost = cost,
                Type = type,
                IsFamilyFriendly = type == EventType.Theater || type == EventType.Concert,
                ArenaId = arenaId,
                SeatLayoutId = layout.Id
            });
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

                tickets.Add(new Ticket
                {
                    Price = totalPrice,
                    EventId = ev.Id,
                    BookableSpaceId = seat.Id,
                    Description = description
                });
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
            var booking1 = new Booking
            {
                ReferenceNumber = Guid.NewGuid().ToString("N"),
                TotalPrice = concertTickets.Sum(t => t.Price),
                IsPaid = true
            };

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
            var payment = new Payment
            {
                PaymentMethod = PaymentMethod.CreditCard,
                BookingId = booking.Id
            };
            await _dbContext.Payments.AddAsync(payment);
        }
        await _dbContext.SaveChangesAsync();
    }
}