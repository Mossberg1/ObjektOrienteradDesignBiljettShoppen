using MediatR;

namespace Application.Features.Tickets.CreateTickets;
/// <summary>
/// Används för att skapa biljetter för ett specifikt evenemang.
/// <para>
/// Skickas via MediatR till CreateTicketsHandler som skapar biljetterna och sparar dem i databasen.
/// </para>
/// </summary>
/// <param name="EventId">ID för det evenemang som biljetterna ska skapas för.</param>

public record CreateTicketsCommand(int EventId) : IRequest<bool>;