using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.GetBookingInvoiceFileName
{
    public class GetBookingInvoiceFileNameHandler : IRequestHandler<GetBookingInvoiceFileNameQuery, string?>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBookingInvoiceFileNameHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<string?> Handle(GetBookingInvoiceFileNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Bookings
                .Where(b => b.ReferenceNumber == request.ReferenceNumber)
                .Select(b => b.InvoicePdfPath)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
