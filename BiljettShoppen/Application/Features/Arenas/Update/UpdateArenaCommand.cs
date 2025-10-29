using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Update;

public record UpdateArenaCommand(
    int Id,
    string Name,
    string Address,
    bool Indoors
    ) : IRequest<Arena?>;
