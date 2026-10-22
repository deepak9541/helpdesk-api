using HelpdeskApi.Enums;

namespace HelpdeskApi.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string CreatedByName { get; set; } = string.Empty;
        public string? AssignedToName { get; set; }
    }
}
