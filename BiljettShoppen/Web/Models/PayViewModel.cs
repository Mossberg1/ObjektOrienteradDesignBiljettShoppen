namespace Web.Models;

public class PayViewModel
{
    public int BookingId { get; set; }
    public string EventName { get; set; }
    public decimal TotalPrice { get; set; }
    public string Email { get; set; } = string.Empty;

    public PayViewModel(int bookingId, string eventName, decimal totalPrice)
    {
        BookingId = bookingId;
        EventName = eventName;
        TotalPrice = totalPrice;
    }
}