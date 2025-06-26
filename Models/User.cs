namespace TicketApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}
