using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Base;

public abstract class BookableSpace : BaseEntity
{
    private decimal _price;

    public decimal Price 
    { 
        get { return _price; }
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(Price));
            _price = value;
        }
    }

    public int EntranceId { get; set; }

    [ForeignKey(nameof(EntranceId))]
    public Entrance EntranceNavigation { get; set; }

    public List<Ticket> TicketsNavigation { get; set; } = [];

    public abstract string GetDescription();

    public bool IsBookableForEvent(int eventId)
    {
        return TicketsNavigation.Any(t => t.EventId == eventId && !t.IsBooked() && !t.IsPending());
    }
}