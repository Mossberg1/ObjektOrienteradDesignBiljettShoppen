using Models.Entities;
using Models.Entities.Base;

namespace Web.Models;

public class BuyTicketViewModel
{
    public Event Event { get; set; }
    public List<BookableSpace> Seats { get; set; }
    public int MaxRow { get; set; }
    public int MaxSeatNumber { get; set; }
}