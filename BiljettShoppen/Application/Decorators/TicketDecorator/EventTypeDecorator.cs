using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Decorators.TicketDecorator
{
    /// <summary>
    /// Decorator för att beräkna priset baserat på evenemangstyp.
    /// </summary>
    public class EventTypeDecorator : TicketDecorator
    {
        private readonly EventType _type;

        public EventTypeDecorator(ITicketComponent wrappedTicket, EventType type) : base(wrappedTicket) 
        {
            _type = type;
        }

        public override decimal GetPrice()
        {
            decimal multiplier = _type switch 
            {
                EventType.Concert => 1.5m,
                EventType.Sports => 1.2m,
                EventType.Theater => 0.9m,
                EventType.ComedyShow => 1.1m,
                _ => 1.0m
            };

            return base.GetPrice() * multiplier;
        }

        public override string GetDescription() => $"{base.GetDescription()}, Evenemangstyp: {_type}";
    }
}
