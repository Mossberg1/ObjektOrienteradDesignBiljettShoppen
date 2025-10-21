using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Events.GetRemainingTickets
{
    public record GetRemainingTicketsQuery(int EventId) : IRequest<RemainingTickets>;
}