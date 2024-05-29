namespace TransactionService.Models
{
    public class Account
    {
        public int Id { get; set; }

        public required string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public required int ClientId { get; set; }
        public bool IsActive { get; set; } = true;
        
    }
}
