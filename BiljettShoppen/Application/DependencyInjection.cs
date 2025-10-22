using System.Reflection;
using Application.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registrerar application projektet i DI containern.
    /// </summary>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<BookingTimer>();
        
        // Lägg till background services
        services.AddHostedService(provider => provider.GetRequiredService<BookingTimer>());

        return services;
    }
}