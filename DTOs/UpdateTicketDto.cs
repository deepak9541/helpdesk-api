using HelpdeskApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HelpdeskApi.DTOs
{
    public class UpdateTicketDto
    {
        [Required, StringLength(150, MinimumLength =5)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(2000, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public TicketPriority Priority { get; set; }
    }
}
