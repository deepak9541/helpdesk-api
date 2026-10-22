using HelpdeskApi.Enums;

namespace HelpdeskApi.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;

        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
