namespace TransactionService.Dtos
{
    public class GetTransactionDto
    {
        public required DateTime To { get; set; }

        public required DateTime From { get; set; }

        public required int AccountId { get; set; }
    }
}
