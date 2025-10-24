using MediatR;
using Models.Entities;

namespace Application.Features.Seats.GetSelectedSeats;

public record GetSelectedSeatTicketsQuery(int[] selectedSeatsIds) : IRequest<List<Ticket>>;