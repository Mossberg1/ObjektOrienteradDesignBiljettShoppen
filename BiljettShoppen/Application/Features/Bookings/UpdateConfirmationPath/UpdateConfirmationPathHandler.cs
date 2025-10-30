using DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateConfirmationPath
{
    public class UpdateConfirmationPathHandler : IRequestHandler<UpdateConfirmationPathCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;

        public UpdateConfirmationPathHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateConfirmationPathCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PdfPath))
                return false;

            request.Booking.ConfirmationPdfPath = request.PdfPath;

            _dbContext.Bookings.Update(request.Booking);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
