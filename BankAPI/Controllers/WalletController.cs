using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletService _walletService;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService)
        {
            _logger = logger;
            _walletService = walletService;
        }

        [HttpGet("GetWalletList/{walletOwner}")]
        public async Task<IActionResult> GetWalletList(string walletOwner)
        {
            var result = await _walletService.GetWalletList(walletOwner);
            return Ok(result);
        }

        [HttpGet("GetWallet/{walletId}")]
        public async Task<IActionResult> GetWallet(string walletId)
        {
            var result = await _walletService.GetWallet(walletId);
            return Ok(result);
        }
    }
}
