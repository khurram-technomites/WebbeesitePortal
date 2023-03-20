using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantUserLogManagementRepo : Repository<RestaurantUserLogManagement>, IRestaurantUserLogManagementRepo
    {
        public RestaurantUserLogManagementRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
