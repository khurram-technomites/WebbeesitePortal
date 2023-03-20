using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantAggregatorWiseSaleRepo:Repository<RestaurantAggregatorWiseSale>, IRestaurantAggregatorWiseSaleRepo
    {
        public RestaurantAggregatorWiseSaleRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
