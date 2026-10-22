using HelpdeskApi.DTOs;
using HelpdeskApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpdeskApi.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private int CurrentUserId =>
      int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketDto dto)
        {
            var result = await _ticketService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;
            var tickets = await _ticketService.GetAllAsync(CurrentUserId, role);
            return Ok(tickets);
        }

        [HttpGet("developers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDevelopers()
        {
            var developers = await _ticketService.GetDevelopersAsync();
            return Ok(developers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            return ticket == null ? NotFound() : Ok(ticket);
        }

        [HttpPatch("{id}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int id, AssignTicketDto dto)
        {
            var result = await _ticketService.AssignAsync(id, dto.DeveloperId);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(result.Ticket);

        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> ChangeStatus(int id, UpdateTicketStatusDto dto)
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;
            var result = await _ticketService.ChangeStatusAsync(id, dto.Status, CurrentUserId, role);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(result.Ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTicketDto dto)
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (role == "Developer")
                return Forbid(); 

            var result = await _ticketService.UpdateAsync(id, dto, CurrentUserId, role);

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return Ok(result.Ticket);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ticketService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(new { message = result.Error });

            return NoContent();
        }
    }


    }
