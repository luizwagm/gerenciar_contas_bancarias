using AccountService.Dtos;

namespace AccountService.Services
{
    public interface IAccountService
    {
        Task<AccountDto> GetByIdAsync(int id);
        Task<int> AddAsync(CreateAccountDto createAccountDto);
        Task<bool> DeleteAsync(int clientId);
        Task<decimal> GetBalanceAsync(int id);
        Task<bool> DeactivateAccountAsync(int id);
        Task<bool> DebitCredit(DebitCreditAccountDto debitCreditAccountDto);
    }
}
