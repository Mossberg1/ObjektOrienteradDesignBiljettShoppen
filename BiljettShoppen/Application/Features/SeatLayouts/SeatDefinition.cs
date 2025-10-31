using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SeatLayouts
{
    public class SeatDefinition
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public SeatType Type { get; set; } 
    }
}
