using AutoMapper;
using PersonService.Data.Repositories;
using PersonService.Dtos;
using PersonService.Models;
using RabbitMQ.Client;

namespace PersonService.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "person_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task<PersonDto> GetByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            return _mapper.Map<PersonDto>(person);
        }

        public async Task<int> AddAsync(CreatePersonDto createPersonDto)
        {
            var person = _mapper.Map<Person>(createPersonDto);
            await _personRepository.AddAsync(person);
            return person.Id;
        }

        public async Task<IEnumerable<PersonDto>> GetAllClientsAsync(string role)
        {
            var clients = await _personRepository.GetAllAsync(role);
            return _mapper.Map<IEnumerable<PersonDto>>(clients);
        }
    }
}
