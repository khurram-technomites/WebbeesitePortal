using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantCategoryWiseSaleRepo:Repository<RestaurantCategoryWiseSale>, IRestaurantCategoryWiseSaleRepo
    {
        public RestaurantCategoryWiseSaleRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
        }
    }
}
