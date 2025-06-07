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
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            await _accountService.RegisterUser(registerDto);
            return Ok(new { message = "Registration successful. Please check your email for verification instructions." });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
        {
            await _accountService.VerifyEmail(verifyEmailDto);
            return Ok(new { message = "Email verified successfully. You can now log in." });
        }
    }
}
