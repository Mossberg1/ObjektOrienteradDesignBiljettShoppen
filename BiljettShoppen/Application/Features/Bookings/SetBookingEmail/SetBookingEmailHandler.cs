using DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.SetBookingEmail
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
