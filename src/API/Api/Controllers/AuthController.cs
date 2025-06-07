using Application.DTO;
using Application.Services.Account;
using Domain.Common.Ids;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("change-password/{userId}")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            var entityId = EntityId.Create(userId);
            await _accountService.ChangePassword(entityId, changePasswordDto);
            return Ok(new { message = "Password changed successfully." });
        }

        [HttpDelete("delete-account/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(Guid userId)
        {
            var entityId = EntityId.Create(userId);
            await _accountService.DeleteUser(entityId);
            return Ok(new { message = "Account deleted successfully." });
        }
    }
}
