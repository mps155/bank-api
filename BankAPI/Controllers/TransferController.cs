using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ILogger<TransferController> _logger;
        private readonly ITransferService _transferService;

        public TransferController(ILogger<TransferController> logger, ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }

        [HttpPost("PostTransference")]
        public async Task<IActionResult> PostTransference([FromBody] TransferencePayload transference)
        {
            var result = await _transferService.PostTransference(transference);
            return Ok(result);
        }

        [HttpPost("PostDeposit")]
        public async Task<IActionResult> PostDeposit([FromBody] DepositPayload deposit)
        {
            var result = await _transferService.PostDeposit(deposit);
            return Ok(result);
        }
    }
}
