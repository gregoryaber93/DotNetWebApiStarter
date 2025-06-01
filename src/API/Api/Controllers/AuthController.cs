using Application.DTO;
using Application.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace DetNetWebApiStarter.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = _accountService.Login(loginUserDto);
            return Ok(token);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerDto)
        {
            _accountService.RegisterUser(registerDto);
            return Ok();
        }
    }
}
