using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
  
        public class RestaurantDeliveryStaffRepo : Repository<RestaurantDeliveryStaff>, IRestaurantDeliveryStaffRepo
        {
            public RestaurantDeliveryStaffRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
            {

            }
        }
    
}
