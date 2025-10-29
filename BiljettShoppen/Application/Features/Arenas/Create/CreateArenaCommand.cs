using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;


namespace Application.Features.Arenas.Create;
public record CreateArenaCommand( // Data container för att skapa en ny arena.
    string Name,
    string Address,
    int NumberOfSeats,
    int NumberOfLoges,
    bool Indoors
) : IRequest<Arena>; // Talar om för MediatR att detta är en request för att skapa en ny arena.

