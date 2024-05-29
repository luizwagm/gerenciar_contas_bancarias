namespace TransactionService.Dtos
{
    public class CreateTransactionDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionType { get; set; }
    }
}

