using BFFService.Data;
using BFFService.Dtos;
using BFFService.Models;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BFFService.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Email == loginDto.Email);

            if (person == null || person.Password != loginDto.Password)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, person.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, person.Role ?? "client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var firebaseToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(person.Email ?? string.Empty);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Person> UserMe(LoginDto loginDto)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Email == loginDto.Email);

            if (person == null || person.Password != loginDto.Password)
            {
                return null;
            }

            return person;
        }
    }
}
