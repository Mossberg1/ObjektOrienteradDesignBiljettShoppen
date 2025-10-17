using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasOne(t => t.BookableSpaceNavigation)
            .WithMany(bs => bs.TicketsNavigation)
            .HasForeignKey(t => t.BookableSpaceId)
            .OnDelete(DeleteBehavior.Restrict);

        // En bokningsbar plats kan bara ha en biljett per evenemang
        builder.HasIndex(t => new { t.EventId, t.BookableSpaceId }).IsUnique();
        
        // Constraints
        builder.ToTable(tb => tb.HasCheckConstraint("CHK_Ticket_Price_NotNegative", "\"Price\" >= 0"));
    }
}