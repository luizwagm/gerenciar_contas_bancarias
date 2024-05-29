using BFFService.Dtos;
using TransactionService.Dtos;

namespace BFFService.Services
{
    public interface IIntegrationService
    {
        Task<string> GetAllClientsAsync(GetAllClientDto getAllClients);
        Task<string> CreateClientAsync(CreateClientDto clientDto);
        Task<string> CreateTransactionAsync(CreateTransactionDto transactionDtoDto);
        Task<string> GetAllAccountsAsync();
        Task<string> CreateAccountAsync(CreateAccountDto accountDto);
        Task<string> DeleteAccountAsync(int accountId);
        Task<string> DeactivateAccountAsync(int accountId);
        Task<string> GetClientByIdAsync(int clientId);
        Task<string> GetAccountByIdAsync(int accountId);
        Task<string> GetAllTransactionAsync(int AccountId, DateTime From, DateTime To);
    }
}