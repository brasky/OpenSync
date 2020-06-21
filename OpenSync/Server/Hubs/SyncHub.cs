using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenSync.Server.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();

        public static string Leader = string.Empty;
    }

    public class SyncHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            GetLeader();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            if (UserHandler.Leader == Context.ConnectionId)
            {
                if (UserHandler.ConnectedIds.Count > 0)
                {
                    UserHandler.Leader = UserHandler.ConnectedIds.First();
                }
                else
                {
                    UserHandler.Leader = string.Empty;
                }
            }
            GetLeader();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SyncVideo(string id, int timestamp, int playlistSize)
        {
            if (Context.ConnectionId != UserHandler.Leader) return;
            await Clients.Others.SendAsync("ReceiveSync", id, timestamp, playlistSize);
        }

        public async Task NewVideo(string id, int timestamp)
        {
            await Clients.All.SendAsync("ReceiveNewVideo", id, timestamp);
        }

        public async Task PauseStatus(bool isPaused, int timestamp)
        {
            if (Context.ConnectionId != UserHandler.Leader) return;
            await Clients.Others.SendAsync("ReceivePauseStatus", isPaused, timestamp);
        }

        public async Task SendPlaylist(List<string> playlist, string id, int timestamp)
        {
            await Clients.Others.SendAsync("ReceivePlaylist", playlist, id, timestamp);
        }

        public async Task PlaylistRequest()
        {
            await Clients.Others.SendAsync("PlaylistRequest");
        }

        public async Task GetLeader()
        {
            if (string.IsNullOrEmpty(UserHandler.Leader))
            {
                UserHandler.Leader = Context.ConnectionId;
            }

                await Clients.All.SendAsync("Leader", UserHandler.Leader);
            
        }
    }
}
