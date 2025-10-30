using DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.UpdateInvoicePath
{
    public class UpdateInvoicePathHandler : IRequestHandler<UpdateInvoicePathCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;

        public UpdateInvoicePathHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateInvoicePathCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FilePath)) 
            {
                return false;
            }

            request.Booking.InvoicePdfPath = request.FilePath;

            _dbContext.Bookings.Update(request.Booking);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
