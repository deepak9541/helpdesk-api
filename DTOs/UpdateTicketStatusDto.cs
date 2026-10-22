using HelpdeskApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HelpdeskApi.DTOs
{
    public class UpdateTicketStatusDto
    {
        [Required]
        public TicketStatus Status { get; set; }
    }
}
