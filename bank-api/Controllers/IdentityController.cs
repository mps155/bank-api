using bank_api.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bank_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(ILogger<IdentityController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ActionName("createUser")]
        public dynamic createUser(newUserModel newUserModel)
        {
            
            return Ok(newUserModel);
        }
    }
}
