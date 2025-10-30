using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EmailServiceStub : IEmailService 
{
    private readonly ILogger<EmailServiceStub> _logger;

    public EmailServiceStub(ILogger<EmailServiceStub> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendBookingConfirmationEmail(string toEmail, string bookingReference)
    {
        return true;
    }
}