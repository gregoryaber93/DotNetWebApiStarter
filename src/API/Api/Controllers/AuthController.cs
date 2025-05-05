using Microsoft.AspNetCore.Mvc;

namespace DetNetWebApiStarter.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return BadRequest();
        }
    }
}
