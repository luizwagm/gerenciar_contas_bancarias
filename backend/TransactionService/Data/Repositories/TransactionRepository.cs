using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TransactionService.Dtos;
using TransactionService.Models;

namespace TransactionService.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(GetTransactionDto transactionDto)
        {
            if (transactionDto.AccountId == 0) {
                return await _context.Transactions
                    .Where(t => t.TransactionDate >= transactionDto.From && 
                                t.TransactionDate <= transactionDto.To)
                    .ToListAsync();
            } else {
                return await _context.Transactions
                    .Where(t => t.TransactionDate >= transactionDto.From && 
                                t.TransactionDate <= transactionDto.To && 
                                t.AccountId == transactionDto.AccountId)
                    .ToListAsync();
            }
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            var get = await _context.Transactions.FindAsync(id);
            
            return get ?? throw new KeyNotFoundException("Transaction not found.");
        }
    }
}
