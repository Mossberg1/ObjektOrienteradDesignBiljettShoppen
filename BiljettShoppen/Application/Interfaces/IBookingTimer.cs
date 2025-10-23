using Models.Entities;

namespace Application.Interfaces;

public interface IBookingTimer
{
    void AddBooking(Booking booking);
    bool RemoveBooking(Booking booking);
}