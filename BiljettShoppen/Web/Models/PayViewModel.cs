namespace Web.Models;

public class PayViewModel
{
    public string BookingReference;
    public string EventName { get; set; }
    public decimal TotalPrice { get; set; }
    public string Email { get; set; } = string.Empty;

    public PayViewModel(string bookingReference, string eventName, decimal totalPrice)
    {
        BookingReference = bookingReference;
        EventName = eventName;
        TotalPrice = totalPrice;
    }
}