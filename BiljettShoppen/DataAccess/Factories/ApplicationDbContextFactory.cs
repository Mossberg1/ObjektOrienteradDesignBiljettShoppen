using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Factories;

/// <summary>
/// Denna klass används för att skapa ett DbContext.
/// När du skapar migrations eller uppdaterar databasen via
/// Entity Framework Core CLI.
/// </summary>
internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string _connectionString = "Host=localhost;Port=5432;Username=admin;Password=admin;Database=ticketshop_db";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(_connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}