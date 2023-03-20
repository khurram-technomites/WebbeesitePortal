using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantAggregatorRepo : Repository<RestaurantAggregator>, IRestaurantAggregatorRepo
    {
        public RestaurantAggregatorRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
