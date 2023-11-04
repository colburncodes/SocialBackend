using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Infrastructure.Exceptions;
using Social.Infrastructure.Interfaces;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountRespository _accountRespository;
        private ILogger<UserController> _logger;

        public UserController(IAccountRespository accountRespository,ILogger<UserController> logger)
        {
            _accountRespository = accountRespository;
            _logger = logger;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CurrentUserAsync()
        {
            try
            {
                var user = await _accountRespository.GetCurrentUserAsync();
                return Ok(new { user });
            }
            catch (InvalidCrendentialsException ex)
            {
                _logger.LogWarning(ex.Message, ex);
                return StatusCode(422, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
