using BFFService.Dtos;
using BFFService.Models;

namespace BFFService.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task<Person> UserMe(LoginDto loginDto);
    }
}
