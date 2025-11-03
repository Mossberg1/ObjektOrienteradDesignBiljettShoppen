using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.GetBookingConfirmationFileName
{
    public class GetBookingConfirmationFileNameHandler : IRequestHandler<GetBookingConfirmationFileNameQuery, string?>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBookingConfirmationFileNameHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<string?> Handle(GetBookingConfirmationFileNameQuery request, CancellationToken cancellationToken)
        {
            var fileName = await _dbContext.Bookings
                .Where(b => b.ReferenceNumber == request.ReferenceNumber)
                .Select(b => b.ConfirmationPdfPath)
                .FirstOrDefaultAsync(cancellationToken);

            return fileName;
        }
    }
}
