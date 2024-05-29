using PersonService.Dtos;

namespace PersonService.Services
{
    public interface IPersonService
    {
        Task<PersonDto> GetByIdAsync(int id);
        Task<int> AddAsync(CreatePersonDto personDto);
        Task<IEnumerable<PersonDto>> GetAllClientsAsync(string role);
    }
}
