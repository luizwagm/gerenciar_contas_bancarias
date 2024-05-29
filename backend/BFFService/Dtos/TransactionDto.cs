namespace BFFService.Dtos
{
    public class TransactionDto
    {
        public required int AccountId { get; set; }

        public string? Description { get; set; }

        public required decimal Amount { get; set; }

        public required DateTime TransactionDate { get; set; }

        public required string TransactionType { get; set; }
    }
}
