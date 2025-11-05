using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace DataAccess.Configurations;

/// <summary>
/// Databas konfiguration för Loge tabellen.
/// </summary>
internal sealed class LogeConfiguration : IEntityTypeConfiguration<Loge>
{
    public void Configure(EntityTypeBuilder<Loge> builder)
    {
        // Constraints
        builder.ToTable(tb => tb.HasCheckConstraint("CHK_Loge_NumberOfPeople_NotNegative", "\"NumberOfPeople\" > 0"));
    }
}