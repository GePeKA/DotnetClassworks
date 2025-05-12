using Auth_BackgroundService.Models;
using Auth_BackgroundService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth_BackgroundService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController (IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var isSuccess = await authService.Login(loginDto);

            return isSuccess
                ? Ok()
                : BadRequest();
        }
    }
}
