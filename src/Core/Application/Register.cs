using Application.DTO;
using Application.Mapper;
using Application.Models;
using Application.Models.Validators;
using Application.Services.Account;
using Application.Services.Email;
using Application.Services.Restaurant;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static partial class Register
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
