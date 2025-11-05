using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Decorators.TicketDecorator
{
    /// <summary>
    /// Decorator för att beräkna pris beroende på om evenemanget är ett familjeevenemang eller inte.
    /// </summary>
    public class FamilyEventDecorator : TicketDecorator
    {
        private readonly bool _isFamilyEvent;

        public FamilyEventDecorator(ITicketComponent wrappedTicket, bool isFamilyEvent) : base(wrappedTicket) 
        {
            _isFamilyEvent = isFamilyEvent;
        }

        public override decimal GetPrice() => _isFamilyEvent ? base.GetPrice() * 0.9m : base.GetPrice();

        public override string GetDescription() 
        {
            var familyEventText = _isFamilyEvent ? "Ja" : "Nej";
            return $"{base.GetDescription()}, Familjeevenemang: {familyEventText}";
        }
    }
}
