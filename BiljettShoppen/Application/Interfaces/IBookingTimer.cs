using Models.Entities;

namespace Application.Interfaces;

public interface IBookingTimer
{
    void AddBooking(Booking booking);
    bool RemoveBooking(string bookingReference);
    Booking? GetBooking(string bookingReference);
}