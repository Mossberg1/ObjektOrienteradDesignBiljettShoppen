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
    
    public List<BookableSpace> BookableSpacesNavigation { get; set; } = new List<BookableSpace>();
    
    public List<Event> EventsNavigation { get; set; } = new List<Event>();
}