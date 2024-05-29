using BFFService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BFFService.Services;
using Microsoft.AspNetCore.Authorization;

namespace BFFService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public AccountController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
        {
            try {
                var result = await _integrationService.CreateAccountAsync(createAccountDto);
                var account = JsonConvert.DeserializeObject<AccountDto>(result);
                return CreatedAtAction(nameof(GetAccountById), new { id = account?.Id }, account);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try {
                var result = await _integrationService.GetAccountByIdAsync(id);
                return Ok(result);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _integrationService.DeleteAccountAsync(id);
            return Ok(result);
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateAccount(int id)
        {
            var result = await _integrationService.DeactivateAccountAsync(id);
            return Ok(result);
        }
    }
}
