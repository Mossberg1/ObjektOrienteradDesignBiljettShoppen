using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Base;

public abstract class BookableSpace : BaseEntity
{
    public decimal Price { get; set; }
    
    public int EntranceId { get; set; }
    
    [ForeignKey(nameof(EntranceId))]
    public Entrance EntranceNavigation { get; set; }
    
    public List<Ticket> TicketsNavigation { get; set; } = [];
}