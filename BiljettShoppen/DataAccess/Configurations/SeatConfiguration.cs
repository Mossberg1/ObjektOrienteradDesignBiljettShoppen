using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

/// <summary>
/// Databas konfiguration för Seat tabellen.
/// </summary>
internal sealed class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        // Constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("CHK_Seat_RowNumber_NotNegative", "\"RowNumber\" >= 0");
            tb.HasCheckConstraint("CHK_Seat_ColNumber_NotNegative", "\"ColNumber\" >= 0");
        });
    }
}