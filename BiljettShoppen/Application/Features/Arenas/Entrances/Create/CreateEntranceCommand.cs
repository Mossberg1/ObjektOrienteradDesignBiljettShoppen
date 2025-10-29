using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Entrances.Create
{
    public record CreateEntranceCommand(
        string Name,
        bool VipEntrance,
        int ArenaId
        ) : IRequest<Entrance>;
}
