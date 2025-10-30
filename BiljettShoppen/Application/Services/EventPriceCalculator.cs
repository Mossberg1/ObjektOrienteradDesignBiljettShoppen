using Models.Enums;

namespace Application.Services
{
    public class EventPriceCalculator
    {
        private const decimal BasePrice = 400m;

        public decimal CalculatePrice(
            EventType type,
            DateTime EventDateTime,
            DateTime ReleaseTicketsDate,
            bool isFamilyFriendly = false)
        {
            decimal multiplier = type switch
            {
                EventType.Concert => 1.5m,
                EventType.Sports => 1.2m,
                EventType.Theater => 0.9m,
                EventType.ComedyShow => 1.1m,
                _ => 1.0m
            };

            decimal price = BasePrice * multiplier;

            if (isFamilyFriendly)
                price *= 0.9m; // 10% rabatt på familjeevent

            DateTime now = DateTime.UtcNow;

            if (now > ReleaseTicketsDate && now < EventDateTime)
            {
                TimeSpan totalSalePeriod = EventDateTime - ReleaseTicketsDate;
                TimeSpan elapsed = now - ReleaseTicketsDate;

                if (elapsed.TotalSeconds >= totalSalePeriod.TotalSeconds / 2)
                {
                    price *= 1.3m; // Ökar priset med 30% efter halva försäljningstiden
                }
            }
            return price;
        }
    }
}
