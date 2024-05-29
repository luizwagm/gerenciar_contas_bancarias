namespace AccountService.Dtos
{
    public class DeleteAccountDto
    {
        public int ClientId { get; set; }
        public string? TransactionNumber { get; set; }
    }
}
