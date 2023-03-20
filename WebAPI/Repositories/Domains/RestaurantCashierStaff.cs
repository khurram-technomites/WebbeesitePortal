using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
  
        public class RestaurantCashierStaffRepo : Repository<RestaurantCashierStaff>, IRestaurantCashierStaffRepo
        {
            public RestaurantCashierStaffRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
            {

            }
        }
    
}
