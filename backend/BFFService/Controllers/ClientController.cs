using Microsoft.AspNetCore.Authorization;
using BFFService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BFFService.Services;

namespace BFFService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public ClientController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetAllClients([FromBody] GetAllClientDto getAllClients)
        {
            try {
                var data = await _integrationService.GetAllClientsAsync(getAllClients);
                return Ok(data);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDto createClientDto)
        {
            try {
                var result = await _integrationService.CreateClientAsync(createClientDto);
                var client = JsonConvert.DeserializeObject<ClientDto>(result);
                return CreatedAtAction(nameof(GetClientById), new { id = client?.Id }, client);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var result = await _integrationService.GetClientByIdAsync(id);
            return Ok(result);
        }
    }
}
