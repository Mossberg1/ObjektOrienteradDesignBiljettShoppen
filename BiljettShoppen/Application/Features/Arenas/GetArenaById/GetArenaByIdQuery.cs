using MediatR;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Arenas.GetArenaById
{
    public record GetArenaByIdQuery(int ArenaId) : IRequest<Arena?>
    {
    }
}
