using Application.DTO;
using Application.Models.Validators;
using Application.Services.Account;
using Application.Services.Restaurant;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static partial class Register
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddFluentValidation(fv => { });
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            return services;
        }
    }
}
