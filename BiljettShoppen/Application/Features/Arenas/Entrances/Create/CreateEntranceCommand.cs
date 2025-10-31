using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.Entities;

namespace Application.Features.Arenas.Entrances.Create
/// <summary>
/// Kommandot används för att skapa en ny <see cref="Entrance"/> för en arena.
/// <para>
/// Skickas via MediatR till en handler som skapar entrén i databasen kopplad till en specifik arena.
/// </para>
/// </summary>
/// <param name="Name">Namnet på entrén.</param>
/// <param name="VipEntrance">Anger om entrén är en VIP-entré (true) eller inte (false).</param>
/// <param name="ArenaId">Unikt Id för arenan.</param>
{
    public record CreateEntranceCommand(
        string Name,
        bool VipEntrance,
        int ArenaId
        ) : IRequest<Entrance>;
}
