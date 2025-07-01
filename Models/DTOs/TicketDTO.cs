namespace TicketApi.Models.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public int UserId { get; set; }

    }
}
