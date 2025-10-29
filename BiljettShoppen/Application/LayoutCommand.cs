using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using Models.Entities.Base;

namespace Application;
    // UpdateSeatLayoutGridCommand.cs
using MediatR;

public class UpdateSeatLayoutGridCommand : IRequest
{
    // Identifierar den specifika layouten att ändra
    public int SeatLayoutId { get; set; }

    // De nya värdena för rutnätet
    public int NewNumberOfRows { get; set; }
    public int NewNumberOfCols { get; set; }
}
