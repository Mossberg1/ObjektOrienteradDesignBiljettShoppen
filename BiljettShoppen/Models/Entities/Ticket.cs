using System;
using Models.Entities.Base;

namespace Models.Entities;

public class Ticket : BaseEntity
{
    public decimal Price { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
