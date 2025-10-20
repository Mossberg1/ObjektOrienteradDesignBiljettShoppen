using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class SeatLayout : BaseEntity
{
    public int NumberOfRows { get; set; }
    public int NumberOfCols { get; set; }   
    
    public int ArenaId { get; set; }
    
    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }

    public List<Seat> SeatsNavigation { get; set; } = [];
    
    public List<Event> EventsNavigation { get; set; } = [];
}