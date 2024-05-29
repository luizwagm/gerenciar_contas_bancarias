using Microsoft.EntityFrameworkCore;
using PersonService.Models;
using PersonService.Dtos;
using MySqlConnector;

namespace PersonService.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonWithAccountsDto>> GetAllAsync(string role)
        {
            var query = @"
                SELECT 
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Email,
                    p.DateOfBirth,
                    p.Password,
                    p.Role,
                    a.AccountNumber, 
                    a.Balance,
                    a.IsActive,
                    t.Amount,
                    t.TransactionType,
                    t.TransactionDate
                FROM 
                    Persons p
                LEFT JOIN 
                    Accounts a ON p.Id = a.ClientId
                LEFT JOIN 
                    Transactions t ON p.Id = t.AccountId
                WHERE 
                    p.Role = @role";
            
            var parameters = new[] { new MySqlParameter("@role", role) };

            var result = await _context.Persons
                .FromSqlRaw(query, parameters)
                .Select(p => new PersonWithAccountsDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    DateOfBirth = p.DateOfBirth,
                    Password = p.Password,
                    Role = p.Role,
                    Accounts = _context.Accounts
                        .Where(a => a.ClientId == p.Id)
                        .Select(a => new AccountDto
                        {
                            AccountNumber = a.AccountNumber,
                            Balance = a.Balance,
                            IsActive = a.IsActive,
                        }).ToList(),
                    Transactions = _context.Transactions
                        .Where(t => t.AccountId == p.Id)
                        .Select(t => new TransactionDto
                        {
                            Amount = t.Amount,
                            TransactionType = t.TransactionType,
                            TransactionDate = t.TransactionDate
                        }).ToList()
                })
                .ToListAsync();

            return result;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            var result = await _context.Persons
                .Include(p => p.Accounts)
                .FirstOrDefaultAsync(p => p.Id == id);

            return result ?? throw new KeyNotFoundException("Person not found.");
        }

        public async Task AddAsync(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
        }
    }
}
