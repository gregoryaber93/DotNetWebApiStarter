using Persistence.EF.DbContexts;

namespace Application.Services.Restaurant
{
    public interface IRestaurantService
    {
        void Test();
    }

    public class RestaurantService : IRestaurantService
    {

        public RestaurantService()
        {
            
        }

        public void Test()
        {
            var k = "a";
        }
    }
}
