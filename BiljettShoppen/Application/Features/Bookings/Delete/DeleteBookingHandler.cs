using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Delete
{
    public class DeleteBookingHandler : IRequestHandler<DeleteBookingCommand, bool>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBookingHandler(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Bookings
                .Where(b => b.ReferenceNumber == request.ReferenceNumber)
                .ExecuteDeleteAsync(cancellationToken);

            return result > 0;
        }
    }
}
