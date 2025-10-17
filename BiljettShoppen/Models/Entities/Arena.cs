using Models.Entities.Base;

namespace Models.Entities;

public class Arena : BaseEntity
{
    public string Address { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats  { get; set; }
    public int NumberOfLoges  { get; set; }
    public bool Indoors  { get; set; }
}