using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DependencyInjection
{
    /// <summary>
    /// Lägger till DataAccess biblioteket till DI container.
    /// Metoden kräver att det finns en ConnectionString i
    /// konfigurationsfilen med namnet "DefaultConnection".
    /// </summary>
    /// <param name="services">Objektet som anropar metoden.</param>
    /// <param name="configuration">Konfigurationsfil som innehåller ConnectionString.</param>
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(connectionString)
        );
        
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>()
        );

        return services;
    }
}