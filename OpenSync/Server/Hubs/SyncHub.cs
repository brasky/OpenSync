using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using OpenSync.Shared.Models;

namespace OpenSync.Server.Hubs
{
    public static class RoomHandler
    {
        //List of all rooms
        public static HashSet<Room> Rooms = new HashSet<Room>();
    }

    public static class UserHandler
    {
        public static IDictionary<string, User> ConnectionUserMapping = new Dictionary<string, User>();
        public static IDictionary<User, Room> UserRoomMapping = new Dictionary<User, Room>();
    }

    public class SyncHub : Hub
    {

        public async override Task OnConnectedAsync()
        {
            string roomName = Context.GetHttpContext().Request.Query["room"];
            var user = new User(Context.ConnectionId);
            UserHandler.ConnectionUserMapping.Add(Context.ConnectionId, user);

            var room = new Room(roomName, user);
            await Groups.AddToGroupAsync(user.ConnectionId, room.Name);

            if (RoomHandler.Rooms.Contains(room))
            {
                RoomHandler.Rooms.TryGetValue(room, out room);
                room.Members.Add(user);
                UserHandler.UserRoomMapping.Add(user, room);
            }
            else
            {
                RoomHandler.Rooms.Add(room);
                UserHandler.UserRoomMapping.Add(user, room);
            }

            await Clients.Group(roomName).SendAsync("Members", room.Members.Select(u => u.Name).ToList());
            await SendLeader(room.Name);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectionUserMapping.TryGetValue(Context.ConnectionId, out User user);
            UserHandler.ConnectionUserMapping.Remove(Context.ConnectionId);

            UserHandler.UserRoomMapping.TryGetValue(user, out Room room);
            UserHandler.UserRoomMapping.Remove(user);

            if (room.Members.Count > 1)
            {
                room.Members.Remove(user);
                if (room.Leader.ConnectionId == user.ConnectionId)
                {
                    room.AssignLeader();
                    await SendLeader(room.Name);
                }
            }
            else
            {
                RoomHandler.Rooms.Remove(room);
            }

            await Groups.RemoveFromGroupAsync(user.ConnectionId, room.Name);
            await Clients.Group(room.Name).SendAsync("Members", room.Members.Select(u => u.Name).ToList());
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SyncVideo(string roomName, string id, int timestamp, int playlistSize)
        {
            var room = RoomHandler.Rooms.Where(r => r.Name == roomName).First();
            if (Context.ConnectionId != room.Leader.ConnectionId) return;
            await Clients.GroupExcept(room.Name, room.Leader.ConnectionId).SendAsync("ReceiveSync", id, timestamp, playlistSize);   
        }

        public async Task NewVideo(string roomName, string id, int timestamp)
        {
            await Clients.Group(roomName).SendAsync("ReceiveNewVideo", id, timestamp);
        }

        public async Task PauseStatus(string roomName, bool isPaused, int timestamp)
        {
            var room = RoomHandler.Rooms.Where(r => r.Name == roomName).First();
            if (Context.ConnectionId != room.Leader.ConnectionId) return;
            await Clients.GroupExcept(roomName, room.Leader.ConnectionId).SendAsync("ReceivePauseStatus", isPaused, timestamp);
        }

        public async Task SendPlaylist(string roomName, List<string> playlist, string id, int timestamp)
        {
            await Clients.GroupExcept(roomName, Context.ConnectionId).SendAsync("ReceivePlaylist", playlist, id, timestamp);
        }

        public async Task PlaylistRequest(string roomName)
        {
            await Clients.GroupExcept(roomName, Context.ConnectionId).SendAsync("PlaylistRequest");
        }

        public async Task SendLeader(string roomName)
        {
            var room = RoomHandler.Rooms.Where(r => r.Name == roomName).First();
            if (string.IsNullOrEmpty(room.Leader.Name))
            {
                room.AssignLeader();
            }

            await Clients.Group(roomName).SendAsync("Leader", room.Leader.Name);

        }

        public async Task SetUsername(string roomName, string name)
        {
            var room = RoomHandler.Rooms.Where(r => r.Name == roomName).First();
            var user = room.Members.Where(u => u.ConnectionId == Context.ConnectionId).First();
            user.Name = name;
            if (room.Leader.ConnectionId == Context.ConnectionId)
            {
                await SendLeader(roomName);
            }
            await Clients.Group(roomName).SendAsync("Members", room.Members.Select(u => u.Name).ToList());
        }
    }
}
