using HelpdeskApi.Data;
using HelpdeskApi.DTOs;
using HelpdeskApi.Entities;
using HelpdeskApi.Enums;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskApi.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }

        public async Task<Ticket?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetByCreatedByAsync(int userId)
        {
            return await _dbSet
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.CreatedById == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByAssignedToAsync(int userId)
        {
            return await _dbSet
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Where(t => t.AssignedToId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            return new DashboardStatsDto
            {
                Total = await _dbSet.CountAsync(),
                Open = await _dbSet.CountAsync(t => t.Status == TicketStatus.Open),
                InProgress = await _dbSet.CountAsync(t => t.Status == TicketStatus.InProgress),
                Resolved = await _dbSet.CountAsync(t => t.Status == TicketStatus.Resolved),
                Closed = await _dbSet.CountAsync(t => t.Status == TicketStatus.Closed)
            };
        }
    }
}
