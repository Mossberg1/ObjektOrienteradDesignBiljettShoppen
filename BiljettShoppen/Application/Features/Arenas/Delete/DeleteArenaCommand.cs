using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Arenas.Delete
{
    public record DeleteArenaCommand(int ArenaId) : IRequest<bool>;
}
