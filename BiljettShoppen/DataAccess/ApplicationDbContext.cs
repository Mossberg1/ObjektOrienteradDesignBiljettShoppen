using System.Reflection;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Base;

namespace DataAccess;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext
{
    // Registrera databas tabeller.
    public DbSet<Arena> Arenas { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookableSpace> BookableSpaces { get; set; }
    public DbSet<Entrance> Entrances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Loge> Loges { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<SeatLayout> SeatLayouts { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}



