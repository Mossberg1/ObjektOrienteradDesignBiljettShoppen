using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class Booking : BaseEntity
{
    public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString();
    
    public decimal TotalPrice { get; set; }
    public bool IsPaid { get; set; }
    
    public Payment? PaymentNavigation { get; set; }

    public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();
}