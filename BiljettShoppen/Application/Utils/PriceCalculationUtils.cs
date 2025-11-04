using Application.Decorators.TicketDecorator;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class PriceCalculationUtils
    {
        public static ITicketComponent DecorateTicket(Event ev, Seat seat)
        {
            ITicketComponent ticketComponent = new TicketComponent(ev);

            ticketComponent = new BookableSpaceDecorator(ticketComponent, seat);
            ticketComponent = new EventTypeDecorator(ticketComponent, ev.Type);
            ticketComponent = new TimeDecorator(ticketComponent, new DateTime(ev.Date, ev.StartTime), ev.ReleaseTicketsDate);
            ticketComponent = new FamilyEventDecorator(ticketComponent, ev.IsFamilyFriendly);

            return ticketComponent;
        }
    }
}
