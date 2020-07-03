using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OpenSync.Shared.Models
{
    public class Room : IEquatable<Room>
    {
        public string Name { get; set; }

        public User Leader { get; set; }

        public List<User> Members { get; set; }

        public Room(string name, User firstUser)
        {
            Name = name;
            Leader = firstUser;
            Members = new List<User> { firstUser };
        }

        public void AssignLeader()
        {
            if (Members.Count == 0)
            {
                Leader.ConnectionId = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(Leader.ConnectionId) || Members.Count == 1 || !Members.Contains(Leader))
            {
                Leader = Members.First();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(string roomName)
        {
            return Name == roomName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Room otherRoom = obj as Room;
            if (otherRoom == null) return false;
            return Name == otherRoom.Name;
        }

        public bool Equals([AllowNull] Room x, [AllowNull] Room y)
        {
            return x.Name == y.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public bool Equals([AllowNull] Room other)
        {
            if (other == null) return false;
            return (Name.Equals(other.Name));
        }
    }
}
