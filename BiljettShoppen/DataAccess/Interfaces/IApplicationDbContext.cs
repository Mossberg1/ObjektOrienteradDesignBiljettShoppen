using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Base;

namespace DataAccess.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Arena> Arenas { get; set; }
    DbSet<Booking> Bookings { get; set; }
    DbSet<BookableSpace> BookableSpaces { get; set; } 
    DbSet<Entrance> Entrances { get; set; }
    DbSet<Event> Events { get; set; }
    DbSet<Loge> Loges { get; set; }
    DbSet<Payment> Payments { get; set; }
    DbSet<Seat> Seats { get; set; }
    DbSet<Ticket> Tickets { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}