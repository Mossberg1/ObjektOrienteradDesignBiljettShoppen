using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

internal sealed class ArenaConfiguration : IEntityTypeConfiguration<Arena>
{
    public void Configure(EntityTypeBuilder<Arena> builder)
    {
        builder.HasMany(a => a.EntrancesNavigation)
            .WithOne(e => e.ArenaNavigation)
            .HasForeignKey(e => e.ArenaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.SeatLayoutsNavigation)
            .WithOne(bs => bs.ArenaNavigation)
            .HasForeignKey(bs => bs.ArenaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.EventsNavigation)
            .WithOne(e => e.ArenaNavigation)
            .HasForeignKey(e => e.ArenaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_Arena_NumberOfSeats_NotNegative", "\"NumberOfSeats\" >= 0");
            tb.HasCheckConstraint("CHK_Arena_NumberOfLoges_NotNegative", "\"NumberOfLoges\" >= 0");
        });
    }
}