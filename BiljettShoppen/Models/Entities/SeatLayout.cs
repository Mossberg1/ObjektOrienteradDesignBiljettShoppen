using Models.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class SeatLayout : BaseEntity
{
    private int _numberOfRows;
    private int _numberOfCols;

    public string Name { get; set; } = string.Empty;
    public int NumberOfRows 
    {
        get { return _numberOfRows; }
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(NumberOfRows));
            _numberOfRows = value;
        }
    }
    public int NumberOfCols 
    {
        get { return _numberOfCols; }
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(NumberOfCols));
            _numberOfCols = value;
        }
    }

    public int ArenaId { get; set; }

    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }

    public List<Seat> SeatsNavigation { get; set; } = [];

    public List<Event> EventsNavigation { get; set; } = [];

    public SeatLayout() { }

    public SeatLayout(string name, int numberOfRows, int numberOfCols, int arenaId) 
    {
        Name = name;
        NumberOfRows = numberOfRows;
        NumberOfCols = numberOfCols;
        ArenaId = arenaId;
    }
}