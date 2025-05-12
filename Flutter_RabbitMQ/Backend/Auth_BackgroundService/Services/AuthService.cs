using Auth_BackgroundService.Models;
using System.Collections.Concurrent;

namespace Auth_BackgroundService.Services
{
    public class AuthService : IAuthService
    {
        private static readonly ConcurrentDictionary<string, string> _sessions = new();

        public Task<bool> Login(LoginDto loginDto)
        {
            if (_sessions.TryRemove(loginDto.Username, out _))
            {
                Console.WriteLine($"Terminated session for user: {loginDto.Username}. Reason: another user with same username loged in");
            }

            _sessions[loginDto.Username] = loginDto.Password;
            return Task.FromResult(true);
        }

        public void Logout(string username)
        {
            _sessions.TryRemove(username, out _);
        }
    }
}
