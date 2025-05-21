using Microsoft.Extensions.DependencyInjection;
using Persistence.EF.DbContexts;

namespace Persistence.EF
{
    public static partial class Register
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<TestDbContext>();
            services.AddDbContext<UserDbContext>();
            return services;
        }
    }
}
