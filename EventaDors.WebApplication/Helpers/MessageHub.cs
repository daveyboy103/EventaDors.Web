
using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace EventaDors.WebApplication.Helpers
{
    [HubName("MessageHub")]
    public class MessageHub : Hub
    { 
        public async Task SendMessage(string sender, string message)
        {
            await Clients.All.SendAsync("SendMessage", sender, message);
        }
        
        public async Task NewMessage(string sender, string message)
        {
            await Clients.All.SendAsync("NewMessage", sender, message);
        }
        
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}