namespace TransactionService.Dtos
{
    public class AccountDto
    {
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }

    public class TransactionWithAccountsDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionType { get; set; }
        public List<AccountDto> Accounts { get; set;} = new();
    }
}
