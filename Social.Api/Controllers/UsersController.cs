using Microsoft.AspNetCore.Mvc;
using Social.Core.Requests;
using Social.Core.Responses;
using Social.Infrastructure.Interfaces;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAccountRespository _accountRespository;
        private ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IAccountRespository accountRespository)
        {
            _logger = logger;
            _accountRespository = accountRespository;
        }
        
        [HttpPost]
        public IActionResult RegisterUser([FromBody] UserRequest<Register> req)
        {
            var user = _accountRespository.RegisterUser(req.User);
            
            return Ok(new {user});
        }
    }
}
