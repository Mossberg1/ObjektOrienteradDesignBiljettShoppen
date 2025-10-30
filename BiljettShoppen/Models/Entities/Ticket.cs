using Models.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class Ticket : BaseEntity
{
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? BookingId { get; set; }

    [ForeignKey(nameof(BookingId))]
    public Booking? BookingNavigation { get; set; }

    public int EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public Event EventNavigation { get; set; }

    public int BookableSpaceId { get; set; }

    [ForeignKey(nameof(BookableSpaceId))]
    public BookableSpace BookableSpaceNavigation { get; set; }

    // Håller reda på om biljetten är bokad men inte betald än.
    public string? PendingBookingReference { get; set; }

    public bool IsPending() => !string.IsNullOrEmpty(PendingBookingReference);

    public bool IsBooked() => BookingId.HasValue;

}
