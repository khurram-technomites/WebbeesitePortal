using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantCashDenominationRepo:Repository<RestaurantCashDenomination>, IRestaurantCashDenominationRepo
    {
        public RestaurantCashDenominationRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
