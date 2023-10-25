using Microsoft.AspNetCore.Mvc;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private ILogger<ProfilesController> _logger;

        public ProfilesController(ILogger<ProfilesController> logger)
        {
            _logger = logger;
        }
    }
}
