namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendBookingEmailWithFileAsync(string toEmail, string bookingReference, byte[] fileBytes);
}