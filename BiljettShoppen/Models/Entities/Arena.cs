using Models.Entities.Base;

namespace Models.Entities;

public class Arena : BaseEntity
{
    public string Address { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats  { get; set; }
    public int NumberOfLoges  { get; set; }
    public bool Indoors  { get; set; }
    
    public List<Entrance> EntrancesNavigation { get; set; } = new List<Entrance>();
    
    public List<Seat> SeatsNavigation { get; set; } = new List<Seat>();
    
    public List<Loge> LogesNavigation { get; set; } = new List<Loge>();
    
    public List<Event> EventsNavigation { get; set; } = new List<Event>();
}