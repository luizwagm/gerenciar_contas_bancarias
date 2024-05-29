namespace BFFService.Dtos
{
    public class LoginDto
    {
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}