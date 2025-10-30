namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendBookingConfirmationEmail(string toEmail, string bookingReference);
}