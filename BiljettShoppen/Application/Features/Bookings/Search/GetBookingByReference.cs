using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Application.Features.Bookings.Search;
/// <summary>
/// Hanterar hämtning av en <see cref="Booking"/> baserat på referensnummer.
/// <para>
/// Tar emot <see cref="GetBookingByReferenceQuery"/> via MediatR och returnerar bokningen <br/>
/// inklusive dess biljetter, event och betalningsinformation.
/// </para>
/// </summary>

public class GetBookingByReferenceHandler : IRequestHandler<GetBookingByReferenceQuery, Booking?>
{
    private readonly IApplicationDbContext _dbContext;

    public GetBookingByReferenceHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Booking?> Handle(GetBookingByReferenceQuery request, CancellationToken cancellationToken)
    {
        var booking = await _dbContext.Bookings
            .Include(b => b.TicketsNavigation)
            .ThenInclude(t => t.BookableSpaceNavigation)
            .Include(b => b.TicketsNavigation)
            .ThenInclude(t => t.EventNavigation)
            .Include(b => b.PaymentNavigation)
            .FirstOrDefaultAsync(b => b.ReferenceNumber == request.ReferenceNumber, cancellationToken);

        return booking;
    }
}