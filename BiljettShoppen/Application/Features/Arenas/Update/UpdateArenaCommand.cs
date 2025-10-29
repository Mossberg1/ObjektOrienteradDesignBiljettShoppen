using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Update;

public record UpdateArenaCommand(
    int Id,
    string Name,
    string Address,
    bool Indoors
    ) : IRequest<Arena?>;
