using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartTeamManagementRepo : Repository<SparePartTeamManagement>, ISparePartTeamManagementRepo
    {
        public SparePartTeamManagementRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
