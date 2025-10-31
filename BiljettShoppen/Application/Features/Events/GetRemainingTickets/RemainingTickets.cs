namespace Application.Features.Events.GetRemainingTickets
/// <summary>
/// Representerar information om kvarvarande biljetter för ett specifikt evenemang.
/// Innehåller evenemangs ID, namn och antal återstående biljetter.
/// </summary>
/// <remarks>
/// Klassen används främst som ett "resultatobjekt" för GetRemainingTicketsQuery, <br/>
/// som hämtar information om hur många biljetter som finns kvar för ett evenemang.
/// </remarks>
{
    public class RemainingTickets
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int TicketsRemaining { get; set; }
    }
}