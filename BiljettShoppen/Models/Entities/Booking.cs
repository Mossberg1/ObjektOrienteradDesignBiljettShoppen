using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class Booking : BaseEntity
{
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    
    public int? PaymentId { get; set; }
    
    [ForeignKey(nameof(PaymentId))]
    public Payment? PaymentNavigation { get; set; }

    public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();
}