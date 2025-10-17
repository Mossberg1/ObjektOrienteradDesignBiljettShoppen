using System;
using Models.Entities.Base;

namespace Models.Entities;

public class Entrance : BaseEntity
{
    public string Name { get; set; }
    public bool VipEntrance { get; set; }
}
