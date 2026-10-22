using HelpdeskApi.Data;

namespace HelpdeskApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ITicketRepository Tickets { get; }
    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext context, ITicketRepository tickets, IUserRepository users)
    {
        _context = context;
        Tickets = tickets;
        Users = users;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}