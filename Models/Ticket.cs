using System.Text.Json.Serialization;

namespace TicketApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } = "open";
        [JsonPropertyName("create_at")]
        public DateTime CreateAt { get;private set; } 
        public int UserId { get; set; }


        public void SetCreateAtNow()
    {
        CreateAt = DateTime.Now;
    }

    }
}
