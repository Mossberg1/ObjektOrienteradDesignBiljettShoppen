using Models.Enums;

namespace Application.Services
{
    public class EventPriceCalculator
    {
        private const decimal BasePrice = 400m;

        public decimal CalculatePrice(EventType type, bool isFamilyFriendly = false)
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

            return price;
        }
    }
}
