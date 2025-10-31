using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SeatLayouts
{
    public record CreateSeatLayoutCommand(
        int ArenaId,
        string LayoutName,
        int NumberOfRows,
        int NumberOfCols,
        decimal ChairBasePrice,
        decimal BenchBasePrice,
        int EntranceId,
        List<SeatDefinition> Seats
    ) : IRequest<int>
    {
    }
}
