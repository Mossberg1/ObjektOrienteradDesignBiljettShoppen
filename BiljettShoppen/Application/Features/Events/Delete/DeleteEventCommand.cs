using MediatR;
using Models.Entities;

namespace Application.Features.Events.Delete;
/// <summary>
/// Kommando för att ta bort <see cref="Event"/> från databasen.
/// <para>
/// Tar endast emot ID för eventet som ska tas bort.
/// </para>
/// </summary>
/// <param name="EventId">Unikt ID för det event som ska tas bort.</param>
public record DeleteEventCommand(int EventId) : IRequest<bool>;
