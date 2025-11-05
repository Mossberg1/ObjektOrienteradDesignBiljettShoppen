using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Base;

/// <summary>
/// Denna abstracta klass blir till en egen tabell i databasen med (TPH) Table Per Hierarchy strategi.
/// Vilket innebär att Seat och Loge delar tabell. Där Loge specifika kolumner blir nullable för Seat.
/// Och tvärtom för Loge. Detta gör att vi kan använda oss av en abstrakt klass i våran kod för
/// att öka flexibiliteten. Det kan låta konstigt att vi ska sätta vissa värden till null i den delade
/// tabellen. Däremot är det inget som märks av i koden och hanteras helt och hållet automatiskt
/// av Entity Framework Core i bakgrunden. Denna strategi är vald för att den ska ge bäst prestanda.
/// </summary>
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