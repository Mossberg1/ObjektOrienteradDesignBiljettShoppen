using Models.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities;

public class Entrance : BaseEntity
{
    public string Name { get; set; }
    public bool VipEntrance { get; set; }

    public int ArenaId { get; set; }

    [ForeignKey(nameof(ArenaId))]
    public Arena ArenaNavigation { get; set; }

    public Entrance() { }

    public Entrance(string name, bool vip, int arenaId) 
    {
        Name = name;
        VipEntrance = vip;
        ArenaId = arenaId;
    }
}
