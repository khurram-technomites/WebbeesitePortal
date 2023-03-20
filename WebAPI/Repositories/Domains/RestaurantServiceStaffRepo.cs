using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    
        public class RestaurantServiceStaffRepo : Repository<RestaurantServiceStaff>, IRestaurantServiceStaffRepo
        {
            public RestaurantServiceStaffRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
            {

            }
        }
    
}
