using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Payment : BaseEntity
{
    public int BookingId { get; set; }
    
    [ForeignKey(nameof(BookingId))]
    public Booking BookingNavigation { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; }

    public string PaymentMethodToString()
    {
        return PaymentMethod.ToString();
    }
}