using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Seat : BookableSpace
{
    public int RowNumber { get; set; }
    public int ColNumber { get; set; }
    public SeatType Type { get; set; }

    public string TypeToString()
    {
        return Type.ToString();
    }
}