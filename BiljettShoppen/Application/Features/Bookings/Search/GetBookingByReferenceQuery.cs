using MediatR;
using Models.Entities;

namespace Application.Features.Bookings.Search;


public record GetBookingByReferenceQuery(
    string ReferenceNumber
) : IRequest<Booking?>;

