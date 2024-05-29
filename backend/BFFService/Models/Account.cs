namespace BFFService.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public required int ClientId { get; set; }
    }
}
