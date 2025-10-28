using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities.Base;
using Models.Enums;

namespace Models.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime ReleaseTicketsDate { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public EventType Type { get; set; } 
        public bool IsFamilyFriendly { get; set; }

        public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();
        
        public int ArenaId { get; set; }
        
        [ForeignKey(nameof(ArenaId))]
        public Arena ArenaNavigation { get; set; }
        
        public int SeatLayoutId { get; set; }
        
        [ForeignKey(nameof(SeatLayoutId))]
        public SeatLayout SeatLayoutNavigation { get; set; }  
        
        public TimeSpan CalculateEventLength()
        {
            return EndTime.ToTimeSpan() - StartTime.ToTimeSpan();
        }

        public decimal CalculateEarnings()
        {
            if (TicketsNavigation.Count == 0)
                return 0 - Cost;

            var soldTickets = TicketsNavigation.Where(t => t.IsBooked()).ToList();
            var totalEarnings = soldTickets.Sum(t => t.Price);

            return totalEarnings - Cost;
        }

        public string TypeToString() => Type.ToString();
    }
}
