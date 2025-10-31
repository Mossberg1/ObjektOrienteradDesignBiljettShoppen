using DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Bookings.SetBookingEmail;

namespace Application.Features.Bookings.SetBookingEmail
/// <summary>
/// Hanterar uppdatering av e-postadressen för en befintlig <see cref="Models.Entities.Booking"/>.
/// <para>
/// Tar emot <see cref="SetBookingEmailCommand"/> via MediatR, validerar e-postadressen <br/>
/// och uppdaterar bokningen i databasen.
/// </para>
/// </summary>
{
    public class SetBookingEmailHandler : IRequestHandler<SetBookingEmailCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;

        public SetBookingEmailHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(SetBookingEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return false;
            }

            request.Booking.Email = request.Email;

            _dbContext.Bookings.Update(request.Booking);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
