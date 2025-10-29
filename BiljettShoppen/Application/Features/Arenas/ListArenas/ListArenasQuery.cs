using MediatR;
using Models.Entities;

namespace Application.Features.Arenas
{
    public record ListArenasQuery() : IRequest<List<Arena>>;
}
