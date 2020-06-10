using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSync.Server.Hubs
{
    public class SyncHub : Hub
    {
        public async Task SyncVideo(string id, int timestamp, int playlistSize)
        {
            await Clients.Others.SendAsync("ReceiveSync", id, timestamp, playlistSize);
        }

        public async Task NewVideo(string id, int timestamp)
        {
            await Clients.All.SendAsync("ReceiveNewVideo", id, timestamp);
        }

        public async Task PauseStatus(bool isPaused, int timestamp)
        {
            await Clients.Others.SendAsync("ReceivePauseStatus", isPaused, timestamp);
        }

        public async Task SendPlaylist(List<string> playlist, string id, int timestamp)
        {
            await Clients.Others.SendAsync("ReceivePlaylist", playlist, id, timestamp);
        }

        public async Task PlaylistRequest(bool _)
        {
            await Clients.Others.SendAsync("PlaylistRequest", true);
        }
    }
}
