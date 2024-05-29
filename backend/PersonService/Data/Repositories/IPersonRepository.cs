using PersonService.Models;
using PersonService.Dtos;

namespace PersonService.Data.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<PersonWithAccountsDto>> GetAllAsync(string role);
        Task<Person> GetByIdAsync(int id);
        Task AddAsync(Person person);
    }
}
