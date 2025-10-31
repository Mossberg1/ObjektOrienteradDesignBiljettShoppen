using DataAccess;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Bookings.UpdateInvoicePath;

namespace Application.Features.Bookings.UpdateInvoicePath
/// <summary>
/// Hanterar uppdatering av fakturans filväg för en befintlig <see cref="Models.Entities.Booking"/>.
/// <para>
/// Tar emot <see cref="UpdateInvoicePathCommand"/> via MediatR, validerar filvägen och uppdaterar bokningen i databasen.
/// </para>
/// </summary>
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
