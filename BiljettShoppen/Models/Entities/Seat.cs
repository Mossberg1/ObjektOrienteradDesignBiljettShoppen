using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities;

public class Seat : BookableSpace
{
    public int RowNumber { get; set; }
    public int ColNumber { get; set; }
    public SeatType Type { get; set; }
    
    public int SeatLayoutId { get; set; }
    
    [ForeignKey(nameof(SeatLayoutId))]
    public SeatLayout SeatLayoutNavigation { get; set; }

    public override string GetDescription()
    {
        return $"Rad: {RowNumber}, Plats: {ColNumber}, Typ: {TypeToString()}";
    }

    public string TypeToString()
    {
        return Type.ToString();
    }
}