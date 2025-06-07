using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO;
using Application.Services.Email;
using AutoMapper;
using Domain.Common.Ids;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.EF.DbContexts;

namespace Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserDbContext _userDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AccountService(
            UserDbContext userDbContext, 
            IPasswordHasher<User> passwordHasher, 
            IValidator<RegisterUserDto> registerUserDotValidator, 
            AuthenticationSettings authenticationSettings, 
            IMapper mapper,
            IEmailSender emailSender)
        {
            _userDbContext = userDbContext;
            _passwordHasher = passwordHasher;
            _registerUserDtoValidator = registerUserDotValidator;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var result = _registerUserDtoValidator.Validate(registerUserDto);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    Console.WriteLine($"Error: {failure.ErrorMessage}");
                }
                throw new ValidationException("Validation failed for the library object.");
            }

            var newUser = _mapper.Map<User>(registerUserDto, opts => opts.Items["UserDbContext"] = _userDbContext);
            newUser.Password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            _userDbContext.User.Add(newUser);
            await _userDbContext.SaveChangesAsync();

            // Send registration confirmation email
            await _emailSender.SendRegistrationConfirmationAsync(newUser.Email, newUser.RegisterCode);
        }

        public async Task VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.Id == verifyEmailDto.Id);
            
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.IsActive)
            {
                throw new Exception("Email is already verified");
            }

            if (user.RegisterCode != verifyEmailDto.RegisterCode)
            {
                throw new Exception("Invalid verification code");
            }

            user.IsActive = true;
            await _userDbContext.SaveChangesAsync();
        }

        public string Login(LoginUserDto loginUserDto)
        {
            var user = _userDbContext.User.Include(u => u.Role).FirstOrDefault(u => u.Email == loginUserDto.Email);
            if (user is null)
            {
                throw new Exception("Wrong mail or password");
            }

            if (!user.IsActive)
            {
                throw new Exception("Account is not activated. Please check your email for activation instructions.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Wrong mail or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task ChangePassword(EntityId userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, changePasswordDto.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Current password is incorrect");
            }

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            {
                throw new Exception("New passwords do not match");
            }

            user.Password = _passwordHasher.HashPassword(user, changePasswordDto.NewPassword);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(EntityId userId)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _userDbContext.User.Remove(user);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task RequestPasswordReset(string email)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return;
            }

            var resetCode = Guid.NewGuid().ToString();
            user.RegisterCode = resetCode;
            await _userDbContext.SaveChangesAsync();

            // Send password reset email
            await _emailSender.SendPasswordResetAsync(user.Email, resetCode);
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.RegisterCode == resetPasswordDto.RegisterCode);
            if (user == null)
            {
                throw new Exception("Invalid reset code");
            }

            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                throw new Exception("Passwords do not match");
            }

            user.Password = _passwordHasher.HashPassword(user, resetPasswordDto.NewPassword);
            user.RegisterCode = Guid.NewGuid().ToString(); // Generate new code after use
            await _userDbContext.SaveChangesAsync();
        }
    }
}