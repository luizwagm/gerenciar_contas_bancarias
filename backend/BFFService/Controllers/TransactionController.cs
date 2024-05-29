using Microsoft.AspNetCore.Authorization;
using BFFService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BFFService.Services;
using TransactionService.Dtos;

namespace BFFService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public TransactionController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpGet("all/{AccountId}/{From}/{To}")]
        public async Task<IActionResult> GetAllTransactions(int AccountId, DateTime From, DateTime To)
        {
            try {
                var data = await _integrationService.GetAllTransactionAsync(AccountId, From, To);
                return Ok(data);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
        {
            try {
                var result = await _integrationService.CreateTransactionAsync(createTransactionDto);
                var transaction = JsonConvert.DeserializeObject<TransactionDto>(result);
                return CreatedAtAction(
                    nameof(GetTransactionById),
                    new {
                        AccountId = createTransactionDto.AccountId,
                        From = createTransactionDto.TransactionDate,
                        To = createTransactionDto.TransactionDate
                    }, transaction);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpGet("{AccountId}/{From}/{To}")]
        public async Task<IActionResult> GetTransactionById(int AccountId, DateTime From, DateTime To)
        {
            var result = await _integrationService.GetAllTransactionAsync(AccountId, From, To);
            return Ok(result);
        }
    }
}
