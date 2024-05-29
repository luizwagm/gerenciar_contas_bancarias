using AccountService.Dtos;
using AccountService.Models;

namespace AccountService.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(int id);
        Task AddAsync(Account account);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeactivateAccountAsync(int id);
        Task<bool> DebitCredit(DebitCreditAccountDto debitCreditAccountDto);
    }
}
