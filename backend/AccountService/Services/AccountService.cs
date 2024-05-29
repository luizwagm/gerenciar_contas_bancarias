using AutoMapper;
using AccountService.Data.Repositories;
using AccountService.Dtos;
using AccountService.Models;

namespace AccountService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto> GetByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<int> AddAsync(CreateAccountDto createAccountDto) // ok
        {
            var account = _mapper.Map<Account>(createAccountDto);
            await _accountRepository.AddAsync(account);
            return account.Id;
        }

        public async Task<bool> DeleteAsync(int clientId)
        {
            return await _accountRepository.DeleteAsync(clientId);
        }

        public async Task<bool> DeactivateAccountAsync(int id)
        {
            return await _accountRepository.DeactivateAccountAsync(id);
        }

        public async Task<bool> DebitCredit(DebitCreditAccountDto debitCreditAccountDto)
        {
            return await _accountRepository.DebitCredit(debitCreditAccountDto);
        }

        public async Task<decimal> GetBalanceAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account?.Balance ?? 0;
        }
    }
}
