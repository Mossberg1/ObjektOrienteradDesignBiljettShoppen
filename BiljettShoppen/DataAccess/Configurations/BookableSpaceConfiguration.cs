using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Base;

namespace DataAccess.Configurations;

/// <summary>
/// Databas konfiguration för BookableSpace tabellen.
/// </summary>
internal sealed class BookableSpaceConfiguration : IEntityTypeConfiguration<BookableSpace>
{
    public void Configure(EntityTypeBuilder<BookableSpace> builder)
    {
        builder.HasOne(bs => bs.EntranceNavigation)
            .WithMany()
            .HasForeignKey(bs => bs.EntranceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(tb => tb.HasCheckConstraint("CHK_BookableSpace_Price_NotNegative", "\"Price\" >= 0"));
    }
}