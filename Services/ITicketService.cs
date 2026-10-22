using HelpdeskApi.DTOs;
using HelpdeskApi.Enums;

namespace HelpdeskApi.Services
{
    public interface ITicketService
    {
        Task<TicketDto> CreateAsync(CreateTicketDto dto, int currentUserId);
        Task<(bool Success, string? Error, TicketDto? Ticket)> AssignAsync(int ticketId, int developerId);
        Task<(bool Success, string? Error, TicketDto? Ticket)> ChangeStatusAsync(
    int ticketId, TicketStatus newStatus, int currentUserId, string role);
        Task<TicketDto?> GetByIdAsync(int id);

        Task<(bool Success, string? Error, TicketDto? Ticket)> UpdateAsync(
     int ticketId, UpdateTicketDto dto, int currentUserId, string role);

        Task<(bool Success, string? Error)> DeleteAsync(int ticketId);

        Task<IEnumerable<TicketDto>> GetAllAsync(int currentUserId, string role);
        Task<IEnumerable<object>> GetDevelopersAsync();

        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
