using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantBranchScheduleRepo : Repository<RestaurantBranchSchedule>, IRestaurantBranchScheduleRepo
    {
        public RestaurantBranchScheduleRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
