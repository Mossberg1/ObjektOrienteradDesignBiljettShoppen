using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities;
using Models.Entities.Base;

public class EventSeatLayout : BaseEntity

{

    public int NumberOfRows { get; set; }

    public int NumberOfCols { get; set; }



    public int ArenaId { get; set; }



    [ForeignKey(nameof(ArenaId))]

    public Arena ArenaNavigation { get; set; }



    public List<Seat> SeatsNavigation { get; set; } = [];



    public List<Event> EventsNavigation { get; set; } = [];

}