using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
