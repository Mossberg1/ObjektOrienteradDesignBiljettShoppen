using System;
using System.Collections.Generic;
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
