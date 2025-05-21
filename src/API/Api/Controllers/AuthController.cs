using Application.Services.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace DetNetWebApiStarter.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public AuthController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            _restaurantService.Test();
            return Ok("asd");
        }

        [HttpPost("register")]
        public IActionResult Register()
        {
            return BadRequest();
        }
    }
}
