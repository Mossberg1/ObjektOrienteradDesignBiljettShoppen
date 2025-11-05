using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    /// <summary>
    /// SignalR hub för att möjliggöra direkt kommunikation mellan server och klient
    /// under bokningsprocessen utifall att bokningstiden har gått ut.
    /// </summary>
    public class BookingHub : Hub
    {
        public async Task JoinBookingGroup(string bookingReference)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, bookingReference);
        }

        public async Task LeaveBookingGroup(string bookingReference)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, bookingReference);
        }
    }
}
