using TransactionService.Dtos;
using TransactionService.Models;

namespace TransactionService.Data.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync(GetTransactionDto transactionDto);
        Task AddAsync(Transaction transaction);

        Task<Transaction> GetByIdAsync(int id);
    }
}
