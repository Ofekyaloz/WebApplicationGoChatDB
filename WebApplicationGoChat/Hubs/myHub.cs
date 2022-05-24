using Microsoft.AspNetCore.SignalR;
namespace WebApplicationGoChat.Hubs
{
    public class myHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}
