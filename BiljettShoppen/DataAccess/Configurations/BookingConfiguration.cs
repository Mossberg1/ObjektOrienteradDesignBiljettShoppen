using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasIndex(b => b.ReferenceNumber)
            .IsUnique();

        builder.HasMany(b => b.TicketsNavigation)
            .WithOne(t => t.BookingNavigation)
            .HasForeignKey(t => t.BookingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}