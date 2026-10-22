using HelpdeskApi.DTOs;

namespace HelpdeskApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    }
}
