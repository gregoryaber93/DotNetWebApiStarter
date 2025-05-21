using Persistence.EF.DbContexts;

namespace Application.Services.Restaurant
{
    public interface IRestaurantService
    {
        void Test();
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly TestDbContext _testDbContext;

        public RestaurantService(TestDbContext dbContext)
        {
            _testDbContext = dbContext;
        }

        public void Test()
        {
            var k = "a";
        }
    }
}
