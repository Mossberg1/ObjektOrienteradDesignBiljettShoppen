using Models.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class SeatLayout : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfRows { get; set; }
    public int NumberOfCols { get; set; }

    public int ArenaId { get; set; }

    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }

    public List<Seat> SeatsNavigation { get; set; } = [];

    public List<Event> EventsNavigation { get; set; } = [];
}