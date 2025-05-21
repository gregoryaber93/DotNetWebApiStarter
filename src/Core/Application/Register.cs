using Application.Services.Restaurant;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static partial class Register
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            return services;
        }
    }
}
