using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Base;

public abstract class BookableSpace : BaseEntity
{
    public decimal Price { get; set; }
    
    public int ArenaId { get; set; }
    
    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }
    
    public int EntranceId { get; set; }
    
    [ForeignKey(nameof(EntranceId))]
    public Entrance EntranceNavigation { get; set; }
}