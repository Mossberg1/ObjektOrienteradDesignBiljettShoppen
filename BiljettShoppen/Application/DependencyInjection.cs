using Application.BackgroundServices;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

        services.AddScoped<IEmailService, EmailServiceStub>();
        services.AddScoped<ICreditCardPaymentService, CreditCardPaymentStubService>();

        return services;
    }
}