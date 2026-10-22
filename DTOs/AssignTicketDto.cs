using System.ComponentModel.DataAnnotations;

namespace HelpdeskApi.DTOs
{
    public class AssignTicketDto
    {
        [Required]
        public int DeveloperId { get; set; }
    }
}
