using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class Ticket : BaseEntity
{
    public decimal Price { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int? BookingId { get; set; }
    
    [ForeignKey(nameof(BookingId))]
    public Booking? BookingNavigation { get; set; }
    
    public int EventId { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public Event EventNavigation { get; set; }
    
    public int? SeatId { get; set; }
    
    [ForeignKey(nameof(SeatId))]
    public Seat? SeatNavigation { get; set; }
    
    public int? LogeId { get; set; }
    
    [ForeignKey(nameof(LogeId))]
    public Loge? LogeNavigation { get; set; }

    public bool IsBooked() => BookingId.HasValue;
}
