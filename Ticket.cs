using System.Text.Json.Serialization;

namespace TicketApi
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } = "open";
        [JsonPropertyName("create_at")]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }

    }
}
