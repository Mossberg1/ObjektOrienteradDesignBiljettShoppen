using MediatR;
using Models.Entities;

namespace Application.Features.Seats.GetSelectedSeats;

public record GetSelectedSeatTicketsQuery(int[] SelectedSeatsIds, int EventId) : IRequest<List<Ticket>>;