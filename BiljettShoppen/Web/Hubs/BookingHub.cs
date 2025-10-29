using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
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
