using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Arenas
{
    public class SeatLayoutDto
    {
            public int Id { get; set; }
            public int ArenaId { get; set; }
            public int NumberOfRows { get; set; }
            public int NumberOfCols { get; set; }
            public int TotalCapacity { get; set; }
        }
}
