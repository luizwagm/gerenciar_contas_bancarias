using AutoMapper;
using TransactionService.Data.Repositories;
using TransactionService.Dtos;
using TransactionService.Models;

namespace TransactionService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync(GetTransactionDto transactionDto)
        {
            var transactions = await _transactionRepository.GetAllAsync(transactionDto);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<int> AddAsync(CreateTransactionDto createTransactionDto)
        {
            var transaction = _mapper.Map<Transaction>(createTransactionDto);
            await _transactionRepository.AddAsync(transaction);
            return transaction.Id;
        }

        public async Task<TransactionDto> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
