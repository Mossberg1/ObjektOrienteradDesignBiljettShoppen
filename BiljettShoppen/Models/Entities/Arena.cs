using Models.Entities.Base;

namespace Models.Entities;


public class Arena : BaseEntity
{
    public string Address { get; set; }
    public string Name { get; set; }
    public bool Indoors { get; set; }

    public List<Entrance> EntrancesNavigation { get; set; } = new List<Entrance>();

    public List<SeatLayout> SeatLayoutsNavigation { get; set; } = [];

    public List<Event> EventsNavigation { get; set; } = new List<Event>();

    public Arena() { }

    public Arena(string name, string address, bool indoors) 
    {
        Name = name;
        Address = address;
        Indoors = indoors;
    }
}