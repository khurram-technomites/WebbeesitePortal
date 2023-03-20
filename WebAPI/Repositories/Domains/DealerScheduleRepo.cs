using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class DealerScheduleRepo : Repository<DealerSchedule> , IDealerScheduleRepo
    {
        public DealerScheduleRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
