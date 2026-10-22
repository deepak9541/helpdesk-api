
using HelpdeskApi.Entities;

namespace HelpdeskApi.Repositories

{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
