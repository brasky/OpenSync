namespace OpenSync.Shared.Models
{
    public class User
    {
        public string ConnectionId { get; set; }

        public Room Room { get; set; }

        public User(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
