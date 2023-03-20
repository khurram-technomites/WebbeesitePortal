using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantCardSchemeRepo : Repository<RestaurantCardScheme>, IRestaurantCardSchemeRepo
    {
        public RestaurantCardSchemeRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
