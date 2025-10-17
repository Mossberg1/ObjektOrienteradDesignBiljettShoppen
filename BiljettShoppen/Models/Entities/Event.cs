using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Event : Base.BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime ReleaseTicketsDate { get; set; }
        public int NumberOfSeatsToSell { get; set; }
        public int NumberOfLogesToSell { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }

        public List<Ticket> TicketsNavigation { get; set; } = new List<Ticket>();
        
        public int ArenaId { get; set; }
        
        [ForeignKey(nameof(ArenaId))]
        public Arena ArenaNavigation { get; set; }
        
        public TimeSpan CalculateEventLength()
        {
            return EndTime.ToTimeSpan() - StartTime.ToTimeSpan();
        }

        public decimal CalculateEarnings()
        {
           return Price - Cost;
        }
    }
}
