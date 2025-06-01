using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO;
using Application.Models.Validators;
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
        void RegisterUser(RegisterUserDto registerUserDto);
        string Login(LoginUserDto loginUserDto);
    }

    public class AccountService : IAccountService
    {
        private readonly UserDbContext _userDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(UserDbContext userDbContext, IPasswordHasher<User> passwordHasher, IValidator<RegisterUserDto> registerUserDotValidator, AuthenticationSettings authenticationSettings)
        {
            _userDbContext = userDbContext;
            _passwordHasher = passwordHasher;
            _registerUserDtoValidator = registerUserDotValidator;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
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

            var newUser = new User()
            {
                Email = registerUserDto.Email,
                Name = registerUserDto.Email,
                RoleId = 2
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            _userDbContext.User.Add(newUser);
            _userDbContext.SaveChanges();
        }

        public string Login(LoginUserDto loginUserDto)
        {
            var user = _userDbContext.User.Include(u => u.Role).FirstOrDefault(u => u.Email == loginUserDto.Email);
            if (user is null)
            {
                throw new Exception("Wrong mail or password");
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