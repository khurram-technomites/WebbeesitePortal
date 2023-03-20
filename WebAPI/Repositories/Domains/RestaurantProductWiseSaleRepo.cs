using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantProductWiseSaleRepo:Repository<RestaurantProductWiseSale>,IRestaurantProductWiseSaleRepo
    {
        public RestaurantProductWiseSaleRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
