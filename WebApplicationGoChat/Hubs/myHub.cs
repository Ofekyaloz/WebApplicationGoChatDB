using Microsoft.AspNetCore.SignalR;
namespace WebApplicationGoChat.Hubs
{
    public class MyHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}
