using Microsoft.EntityFrameworkCore;
using AccountService.Models;
using AccountService.Dtos;

namespace AccountService.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            var account =  await _context.Accounts.FindAsync(id);
            
            return account ?? throw new KeyNotFoundException("Account not found.");
        }

        public async Task AddAsync(Account account)
        {
            var existingAccount = await _context.Accounts
                .FirstOrDefaultAsync(b => b.ClientId == account.ClientId);

            if (existingAccount == null) {
                await _context.Accounts.AddAsync(account);
            } else {
                existingAccount.AccountNumber = account.AccountNumber;
                existingAccount.Balance = account.Balance;
                _context.Accounts.Update(existingAccount);
            }

            await _context.SaveChangesAsync();          
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var accountsToDelete = await _context.Accounts
                    .Where(a => a.ClientId == id && a.Balance <= 0)
                    .ToListAsync();

                Console.WriteLine($"Found {accountsToDelete.Count} accounts to delete for ClientId {id}");

                if (accountsToDelete.Any()) {
                    _context.Accounts.RemoveRange(accountsToDelete);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Deleted {accountsToDelete.Count} accounts for ClientId {id}");

                    return true;
                }

                Console.WriteLine($"No accounts found to delete for ClientId {id}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting accounts for ClientId {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeactivateAccountAsync(int id)
        {
            try
            {
                var accountsToDelete = await _context.Accounts
                    .Where(a => a.ClientId == id && a.Balance >= 0)
                    .FirstOrDefaultAsync();

                if (accountsToDelete != null) {
                    accountsToDelete.IsActive = false;
                    _context.Accounts.Update(accountsToDelete);
                    await _context.SaveChangesAsync();

                    return true;
                }

                Console.WriteLine($"No accounts found to deactivate for ClientId {id}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deactivate accounts for ClientId {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DebitCredit(DebitCreditAccountDto debitCreditAccountDto)
        {
            var account = await _context.Accounts
                .Where(a => a.ClientId == debitCreditAccountDto.AccountId)
                .FirstOrDefaultAsync();
            
            if (account != null) {
                if (debitCreditAccountDto.TransactionType == "debit") {
                    account.Balance -= debitCreditAccountDto.Amount;
                } else {
                    account.Balance += debitCreditAccountDto.Amount;
                }

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}