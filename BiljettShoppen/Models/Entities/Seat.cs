using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Seat : BaseEntity
{
    public int Row { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public SeatType Type { get; set; }

    public string TypeToString()
    {
        return Type.ToString();
    }
}