namespace Contactly.Core.Entities
{
    public class ConnectionTracker
    {
        public string ConnectionId { get; set; }
        public int? ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
