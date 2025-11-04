using Models.Entities.Base;
using Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class Seat : BookableSpace
{
    private int _rowNumber;
    private int _colNumber;

    public int RowNumber 
    { 
        get { return _rowNumber; } 
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(RowNumber));
            _rowNumber = value;
        }
    }
    public int ColNumber 
    {
        get { return _colNumber; } 
        set 
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(ColNumber));
            _colNumber = value;
        }
    }
    public SeatType Type { get; set; }

    public int SeatLayoutId { get; set; }

    [ForeignKey(nameof(SeatLayoutId))]
    public SeatLayout SeatLayoutNavigation { get; set; }

    public Seat() { }

    public Seat(int rowNumber, int colNumber, SeatType type, int seatLayoutId, decimal price, int entranceId) 
    {
        RowNumber = rowNumber;
        ColNumber = colNumber;
        Type = type;
        SeatLayoutId = seatLayoutId;
        Price = price;
        EntranceId = entranceId;
    }

    public override string GetDescription()
    {
        return $"Rad: {RowNumber}, Plats: {ColNumber}, Typ: {TypeToString()}";
    }

    public string TypeToString()
    {
        return Type.ToString();
    }
}