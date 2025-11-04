using Models.Entities.Base;
using Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Event : BaseEntity
    {
        private DateOnly _date;
        private TimeOnly _startTime;
        private TimeOnly _endTime;
        private decimal _price;
        private decimal _cost;

        public string Name { get; set; } = string.Empty;
        public DateOnly Date 
        { 
            get 
            {
                return _date;
            }
            set 
            {
                if (value < DateOnly.FromDateTime(DateTime.UtcNow))
                    throw new ArgumentException("Evenemangets datum måste vara i framtiden.");

                _date = value;
            }
        }
        public TimeOnly StartTime 
        {
            get 
            {
                return _startTime;
            }
            set 
            {
                if (EndTime != default && value >= EndTime)
                    throw new ArgumentException("Starttiden måste vara före sluttiden.");

                _startTime = value;
            }
        }
        public TimeOnly EndTime 
        {
            get 
            {
                return _endTime;
            } 
            set 
            {
                if (StartTime != default && value <= StartTime)
                    throw new ArgumentException("Sluttiden måste vara efter starttiden.");

                _endTime = value;
            }
        }
        public DateTime ReleaseTicketsDate { get; set; }
        public decimal Price 
        { 
            get 
            {
                return _price;
            }
            set 
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                _price = value;
            } 
        
        }
        public decimal Cost 
        { 
            get { return _cost; } 
            set 
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                _cost = value;
            }
        }
        public EventType Type { get; set; }
        public bool IsFamilyFriendly { get; set; }

        public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();

        public int ArenaId { get; set; }

        [ForeignKey(nameof(ArenaId))]
        public Arena ArenaNavigation { get; set; }

        public int SeatLayoutId { get; set; }

        [ForeignKey(nameof(SeatLayoutId))]
        public SeatLayout SeatLayoutNavigation { get; set; }

        public Event() { }

        public Event(
            string name, 
            DateOnly date,
            TimeOnly startTime, 
            TimeOnly endTime, 
            DateTime releaseTicketsDate, 
            decimal price, 
            decimal cost, 
            EventType type, 
            bool isFamilyFriendly,
            int arenaId,
            int seatLayoutId
        ) 
        {
            Name = name;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            ReleaseTicketsDate = releaseTicketsDate;
            Price = price;
            Cost = cost;
            Type = type;
            IsFamilyFriendly = isFamilyFriendly;
            ArenaId = arenaId;
            SeatLayoutId = seatLayoutId;
        }

        public TimeSpan CalculateEventLength()
        {
            return _endTime.ToTimeSpan() - _startTime.ToTimeSpan();
        }

        public decimal CalculateEarnings()
        {
            if (TicketsNavigation.Count == 0)
                return 0 - _cost;

            var soldTickets = TicketsNavigation.Where(t => t.IsBooked()).ToList();
            var totalEarnings = soldTickets.Sum(t => t.Price);

            return totalEarnings - _cost;
        }

        public string TypeToString() => Type.ToString();
    }
}
