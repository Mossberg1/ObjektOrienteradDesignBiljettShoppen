using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.GetSeatLayoutsForArena
{
    public record GetSeatLayoutForArenaIdQuery(int ArenaId) : IRequest<List<SeatLayout>>; 
}


