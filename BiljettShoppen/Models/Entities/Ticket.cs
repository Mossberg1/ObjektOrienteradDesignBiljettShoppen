using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class Ticket : BaseEntity
{
    public decimal Price { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
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

    public bool IsBooked() => BookingId.HasValue;
}
