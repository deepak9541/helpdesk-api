namespace HelpdeskApi.Repositories;

public interface IUnitOfWork
{
    ITicketRepository Tickets { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();
}