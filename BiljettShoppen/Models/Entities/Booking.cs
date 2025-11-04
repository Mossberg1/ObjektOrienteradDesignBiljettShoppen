using Models.Entities.Base;

namespace Models.Entities;

public class Booking : BaseEntity
{
    private decimal _totalPrice;

    public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = string.Empty;
    public decimal TotalPrice 
    {
        get { return _totalPrice; }
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            _totalPrice = value;
        } 
    }
    public bool IsPaid { get; set; }
    public string ConfirmationPdfPath { get; set; } = string.Empty;
    public string? InvoicePdfPath { get; set; }
    public Payment? PaymentNavigation { get; set; }
    public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();

    public Booking() { }

    public Booking(string email, decimal totalPrice, bool isPaid) 
    {
        Email = email;
        TotalPrice = totalPrice;
        IsPaid = isPaid;
    }

    public Booking(decimal totalPrice, List<Ticket> tickets) 
    {
        TotalPrice = totalPrice;
        TicketsNavigation = tickets;
    }
}