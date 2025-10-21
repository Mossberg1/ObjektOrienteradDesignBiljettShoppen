namespace Application.Features.Events.GetRemainingTickets
{
    public class RemainingTickets
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TicketsRemaining { get; set; }
    }
}