namespace BFFService.Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}