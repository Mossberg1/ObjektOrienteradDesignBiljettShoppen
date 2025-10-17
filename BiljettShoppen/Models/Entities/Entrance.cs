using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entities.Base;

namespace Models.Entities;

public class Entrance : BaseEntity
{
    public string Name { get; set; }
    public bool VipEntrance { get; set; }
    
    public int ArenaId { get; set; }
    
    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }
}
