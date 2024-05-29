using TransactionService.Dtos;

namespace TransactionService.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync(GetTransactionDto transactionDto);
        Task<int> AddAsync(CreateTransactionDto createTransactionDto);
        Task<TransactionDto> GetByIdAsync(int id);
    }
}
