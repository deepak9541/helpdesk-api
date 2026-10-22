using HelpdeskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpdeskApi.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {

        private readonly ITicketService _ticketService;
        public DashboardController(ITicketService ticketService) {
            _ticketService = ticketService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _ticketService.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}
