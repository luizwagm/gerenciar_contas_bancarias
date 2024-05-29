using Microsoft.AspNetCore.Mvc;
using BFFService.Dtos;
using BFFService.Services;

namespace BFFService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized();
            }
            var person = await _authService.UserMe(loginDto);

            return Ok(new { Token = token, Type = person.Role, Id = person.Id });
        }
    }
}
