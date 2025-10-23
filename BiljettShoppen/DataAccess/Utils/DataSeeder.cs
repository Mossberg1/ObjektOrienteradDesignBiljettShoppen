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
            return; // DB has been seeded
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
            new() { NumberOfRows = 5, NumberOfCols = 10, ArenaId = arenas[0].Id }, // 50 seats
            new() { NumberOfRows = 7, NumberOfCols = 11, ArenaId = arenas[1].Id }, // 77 seats
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
                        RowNumber = row, ColNumber = col,
                        Type = isBenchRow ? SeatType.Bench : SeatType.Chair,
                        Price = isBenchRow ? 200 : 250, SeatLayoutId = layout.Id, EntranceId = regularEntrance.Id
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
                ReleaseTicketsDate = DateTime.UtcNow.AddDays(10), NumberOfSeatsToSell = 50, NumberOfLogesToSell = 4,
                Price = 300, Cost = 5000, Type = EventType.Concert, IsFamilyFriendly = true, ArenaId = arenas[0].Id,
                SeatLayoutId = seatLayouts[0].Id
            },
            new()
            {
                Name = "Stand-up kväll", Date = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(3)),
                StartTime = new TimeOnly(20, 0, 0), EndTime = new TimeOnly(21, 30, 0),
                ReleaseTicketsDate = DateTime.UtcNow.AddDays(20), NumberOfSeatsToSell = 77, NumberOfLogesToSell = 0,
                Price = 450, Cost = 8000, Type = EventType.ComedyShow, IsFamilyFriendly = false, ArenaId = arenas[1].Id,
                SeatLayoutId = seatLayouts[1].Id
            }
        };

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
                tickets.Add(new Ticket
                {
                    Price = seat.Price,
                    EventId = ev.Id,
                    BookableSpaceId = seat.Id
                });
            }
        }

        await _dbContext.Tickets.AddRangeAsync(tickets);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<List<Booking>> SeedBookingsAsync(List<Event> events)
    {
        var bookings = new List<Booking>();

        // Booking 1: 2 tickets for the concert
        var concert = events.First(e => e.Type == EventType.Concert);
        var concertTickets = await _dbContext.Tickets
            .Include(t => t.BookableSpaceNavigation)
            .Where(t => t.EventId == concert.Id && t.BookingId == null)
            .Skip(2)
            .Take(2).ToListAsync();

        if (concertTickets.Count == 2)
        {
            var booking1 = new Booking { TotalPrice = concertTickets.Sum(t => t.Price), IsPaid = true };
            bookings.Add(booking1);
            concertTickets[0].FirstName = "Anna";
            concertTickets[0].LastName = "Andersson";
            concertTickets[0].BookingNavigation = booking1;
            concertTickets[1].FirstName = "Erik";
            concertTickets[1].LastName = "Andersson";
            concertTickets[1].BookingNavigation = booking1;
        }

        foreach (var ticket in concertTickets)
        {
            ticket.BookableSpaceNavigation.IsBooked = true;
            _dbContext.BookableSpaces.Update(ticket.BookableSpaceNavigation);
        }

        await _dbContext.Bookings.AddRangeAsync(bookings);
        await _dbContext.SaveChangesAsync();
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