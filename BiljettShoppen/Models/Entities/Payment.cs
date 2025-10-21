using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Payment : BaseEntity
{
    public PaymentMethod Method { get; set; }
    
    public int BookingId { get; set; }
    
    [ForeignKey(nameof(BookingId))]
    public Booking BookingNavigation { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public string PaymentMethodToString()
    {
        return Method.ToString();
    }

    public static implicit operator Payment(Payment v)
    {
        throw new NotImplementedException();
    }
}