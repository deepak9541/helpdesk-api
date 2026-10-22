using HelpdeskApi.DTOs;
using HelpdeskApi.Entities;

namespace HelpdeskApi.Repositories
{
    public interface ITicketRepository: IGenericRepository<Ticket>
    {
        Task<Ticket?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Ticket>> GetByCreatedByAsync(int userId);
        Task<IEnumerable<Ticket>> GetByAssignedToAsync(int userId);
        Task<IEnumerable<Ticket>> GetAllWithDetailsAsync();


        Task<DashboardStatsDto> GetStatsAsync();
    }
}
