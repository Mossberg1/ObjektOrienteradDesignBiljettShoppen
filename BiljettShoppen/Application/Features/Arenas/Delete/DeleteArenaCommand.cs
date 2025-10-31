using MediatR;

namespace Application.Features.Arenas.Delete
/// <summary>
/// Kommandot används för att ta bort en arena baserat på en unik ArenaId.
/// <para>
/// Skickas via MediatR till DeleteArenaHandler som tar bort arenan från databasen.
/// </para>
/// </summary>
{
    public record DeleteArenaCommand(int ArenaId) : IRequest<bool>;
}
