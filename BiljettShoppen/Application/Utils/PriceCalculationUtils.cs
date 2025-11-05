using Application.Decorators.TicketDecorator;
using Models.Entities;
using Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class PriceCalculationUtils
    {
        /// <summary>
        /// Utility metod för att dekorera en ticket, vilket gör
        /// att prisberäkningen och beskrivningen av ticketen blir korrekt.
        /// Denna prisberäkning kan användas i de klasser som behöver räkna ut pristet.
        /// Samtidigt som skulle denna beräkning behöva ändras, behöver den enbart ändras här.
        /// </summary>
        /// <param name="ev">De evenemang som biljetten tillhör</param>
        /// <param name="bookableSpace">Den stol som biljetten tillhör</param>
        /// <returns>ITicketComopoent interface för att inte vara beroende av implementation</returns>
        public static ITicketComponent DecorateTicket(Event ev, BookableSpace bookableSpace)
        {
            ITicketComponent ticketComponent = new TicketComponent(ev);

            ticketComponent = new BookableSpaceDecorator(ticketComponent, bookableSpace);
            ticketComponent = new EventTypeDecorator(ticketComponent, ev.Type);
            ticketComponent = new TimeDecorator(ticketComponent, DateTimeUtils.ToUtc(new DateTime(ev.Date, ev.StartTime)), ev.ReleaseTicketsDate);
            ticketComponent = new FamilyEventDecorator(ticketComponent, ev.IsFamilyFriendly);

            return ticketComponent;
        }
    }
}
