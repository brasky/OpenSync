using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSync.Server.Hubs
{
    public class SyncHub : Hub
    {
        public async Task SyncVideo(string id, int timestamp)
        {
            await Clients.All.SendAsync("ReceiveSync", id, timestamp);
        }

        public async Task NewVideo(string id)
        {
            await Clients.All.SendAsync("ReceiveNewVideo", id);
        }

        public async Task PauseStatus(bool isPaused, int timestamp)
        {
            await Clients.All.SendAsync("ReceivePauseStatus", isPaused, timestamp);
        }
    }
}
