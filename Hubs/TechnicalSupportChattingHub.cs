using Fastdo.API.Mappings;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Hubs
{
    public class TechnicalSupportChattingHub:Hub
    {
        private static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public override Task OnConnectedAsync()
        {
            string userId = GetUserId();
            if (!Connections.GetConnections(userId).Contains(Context.ConnectionId))
            {
                Connections.Add(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = GetUserId();
            Connections.Remove(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            return Context.User.Identity.Name;
        }

        public static IEnumerable<string> GetUserConnections(string USerId)
        {
            return Connections.GetConnections(USerId);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
