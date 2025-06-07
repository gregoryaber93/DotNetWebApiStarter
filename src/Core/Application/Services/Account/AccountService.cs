using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO;
using Application.Services.Email;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.EF.DbContexts;

namespace Application.Services.Account
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);
        string Login(LoginUserDto loginUserDto);
        Task VerifyEmail(VerifyEmailDto verifyEmailDto);
    }

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

            var newUser = _mapper.Map<User>(registerUserDto);
            newUser.Password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            _userDbContext.User.Add(newUser);
            await _userDbContext.SaveChangesAsync();

            // Send registration confirmation email
            await _emailSender.SendRegistrationConfirmationAsync(newUser.Email, newUser.RegisterCode);
        }

        public async Task VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var user = await _userDbContext.User.FirstOrDefaultAsync(u => u.Email == verifyEmailDto.Email);
            
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
    }
}