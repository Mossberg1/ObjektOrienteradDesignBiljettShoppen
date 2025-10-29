using Models.Entities;

namespace Web.Models;

public class BuySeatTicketViewModel
{
    public Event Event { get; set; }
    public List<Seat> Seats { get; set; }
    public int MaxRow { get; set; }
    public int MaxSeatNumber { get; set; }
}