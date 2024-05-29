namespace AccountService.Dtos
{
    public class DebitCreditAccountDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionType { get; set; }
    }
}
