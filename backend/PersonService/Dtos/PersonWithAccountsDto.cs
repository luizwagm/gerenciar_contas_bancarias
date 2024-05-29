namespace PersonService.Dtos
{
    public class AccountDto
    {
        public required string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }

    public class TransactionDto
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? TransactionType { get; set; }
    }
    public class PersonWithAccountsDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public List<AccountDto> Accounts { get; set;} = new();
        public List<TransactionDto> Transactions { get; set;} = new();
    }
}
