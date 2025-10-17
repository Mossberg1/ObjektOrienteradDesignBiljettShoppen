using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Payment : BaseEntity
{
    public PaymentMethod Method { get; set; }
    
    public Booking BookingNavigation { get; set; }

    public string PaymentMethodToString()
    {
        return Method.ToString();
    }
}