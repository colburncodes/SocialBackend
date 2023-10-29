using Microsoft.AspNetCore.Mvc;
using Social.Core.Requests;
using Social.Infrastructure.Exceptions;
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
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest<Register> req)
        {
            try
            {
                var user = await _accountRespository.RegisterUserAsync(req.User);
                return Ok(new { user });
            }
            catch (UserExistException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(422);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRequest<Login> req)
        {
            try
            {
                var user = await _accountRespository.LoginUserAsync(req.User);
                return Ok(new { user });
            }
            catch (LoginFailedException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}
