using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantCashDenominationDetailRepo:Repository<RestaurantCashDenominationDetail>, IRestaurantCashDenominationDetailRepo
    {
        public RestaurantCashDenominationDetailRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
