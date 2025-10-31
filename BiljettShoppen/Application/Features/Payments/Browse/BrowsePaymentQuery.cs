using MediatR;
using Models.Entities;

namespace Application.Features.Payments.PayBooking;
/// <summary>
/// Query för att hämta alla betalningar.
/// <para>
/// Skickas via MediatR till BrowsePaymentHandler som returnerar en lista av <see cref="Payment"/>.
/// </para>
/// </summary>
public record BrowsePaymentQuery() : IRequest<List<Payment>>;