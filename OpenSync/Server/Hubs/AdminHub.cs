using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using OpenSync.Shared.Models;

namespace OpenSync.Server.Hubs
{
    public class AdminHub : Hub
    {
        public async Task GetAllRooms()
        {
            var roomsList = RoomHandler.Rooms.Select(r => r.Name).ToList();
            await Clients.Caller.SendAsync("ReceiveRooms", roomsList);
        }
    }
}
