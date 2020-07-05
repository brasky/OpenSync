namespace OpenSync.Shared.Models
{
    public class User
    {
        public string Name { get; set; }

        public string ConnectionId { get; set; }

        public Room Room { get; set; }

        public User(string connectionId)
        {
            ConnectionId = connectionId;
            Name = ConnectionId;
        }

        public User(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }
    }
}
