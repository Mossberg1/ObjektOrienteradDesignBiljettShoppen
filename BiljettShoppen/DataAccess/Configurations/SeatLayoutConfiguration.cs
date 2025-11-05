using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

/// <summary>
/// Databas konfiguration för SeatLayout tabellen.
/// </summary>
internal sealed class SeatLayoutConfiguration : IEntityTypeConfiguration<SeatLayout>
{
    public void Configure(EntityTypeBuilder<SeatLayout> builder)
    {
        builder.HasMany(sl => sl.SeatsNavigation)
            .WithOne(s => s.SeatLayoutNavigation)
            .HasForeignKey(s => s.SeatLayoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sl => sl.EventsNavigation)
            .WithOne(e => e.SeatLayoutNavigation)
            .HasForeignKey(s => s.SeatLayoutId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}