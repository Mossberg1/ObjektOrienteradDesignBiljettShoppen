using System.Reflection;
using Application.BackgroundServices;
using Application.Interfaces;
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

        services.AddSingleton<IBookingTimer, BookingTimer>();
        
        // Lägg till background services
        services.AddHostedService(provider => (BookingTimer)provider.GetRequiredService<IBookingTimer>());

        return services;
    }
}