using Auth_BackgroundService.Models;

namespace Auth_BackgroundService.Services
{
    public interface IAuthService
    {
        Task<bool> Login(LoginDto loginDto);
        void Logout(string username);
    }
}
