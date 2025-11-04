using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Decorators.TicketDecorator
{
    public class TimeDecorator : TicketDecorator
    {
        private readonly DateTime _eventStartDate;
        private readonly DateTime _ticketsReleaseDate;
        private decimal _priceIncreaseFactor = 1.3m;

        public TimeDecorator(ITicketComponent wrappedTicket, DateTime eventStartDate, DateTime ticketsReleaseDate) : base(wrappedTicket)
        {
            _eventStartDate = eventStartDate;
            _ticketsReleaseDate = ticketsReleaseDate;
        }

        public TimeDecorator(ITicketComponent wrappedTicket, DateTime eventStartDate, DateTime ticketsReleaseDate, decimal priceIncreaseFactor) : base(wrappedTicket)
        {
            _eventStartDate = eventStartDate;
            _ticketsReleaseDate = ticketsReleaseDate;
            _priceIncreaseFactor = priceIncreaseFactor;
        }

        public override decimal GetPrice() 
        {
            var now = DateTime.UtcNow;

            if (now > _ticketsReleaseDate && now < _eventStartDate)
            {
                var totalSalePeriod = _eventStartDate - _ticketsReleaseDate;
                var elapsedTime = now - _ticketsReleaseDate;

                if (elapsedTime.TotalSeconds >= totalSalePeriod.TotalSeconds / 2)
                    return base.GetPrice() * _priceIncreaseFactor;
            }

            return base.GetPrice();
        }

        public override string GetDescription() => $"{base.GetDescription()}, Starttid: {_eventStartDate}";
    }
}
