using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasMany(e => e.TicketsNavigation)
            .WithOne(t => t.EventNavigation)
            .HasForeignKey(t => t.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_Event_Date_InFuture", "\"Date\" > CURRENT_TIMESTAMP");
            tb.HasCheckConstraint("CHK_Event_EndTime_AfterStartTime", "\"EndTime\" > \"StartTime\"");
            tb.HasCheckConstraint("CHK_Event_ReleaseTicketsDate_BeforeEventDate", "\"ReleaseTicketsDate\" < \"Date\"");
            tb.HasCheckConstraint("CHK_Event_NumberOfSeatsToSell_NotNegative", "\"NumberOfSeatsToSell\" >= 0");
            tb.HasCheckConstraint("CHK_Event_NumberOfLogesToSell_NotNegative", "\"NumberOfLogesToSell\" >= 0");
            tb.HasCheckConstraint("CHK_Event_HasSomethingToSell", "\"NumberOfSeatsToSell\" > 0 OR \"NumberOfLogesToSell\" > 0");
            tb.HasCheckConstraint("CHK_Event_Price_NotNegative", "\"Price\" >= 0");
            tb.HasCheckConstraint("CHK_Event_Cost_NotNegative", "\"Cost\" >= 0");
        });
    }
}