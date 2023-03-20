using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantTransactionHistoryRepo : Repository<RestaurantTransactionHistory>, IRestaurantTransactionHistoryRepo
    {
        public RestaurantTransactionHistoryRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
