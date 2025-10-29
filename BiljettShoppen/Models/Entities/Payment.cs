using Models.Entities.Base;
using Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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