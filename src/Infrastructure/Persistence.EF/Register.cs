using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EF.DbContexts;

namespace Persistence.EF
{
    public static partial class Register
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("UserConnectionString"), b => b.MigrationsAssembly("Persistence.EF"));
            });
            return services;
        }
    }
}
