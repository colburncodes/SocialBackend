using Microsoft.AspNetCore.Mvc;

namespace Social.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private ILogger<TagsController> _logger;

        public TagsController(ILogger<TagsController> logger)
        {
            _logger = logger;
        }
    }
}
