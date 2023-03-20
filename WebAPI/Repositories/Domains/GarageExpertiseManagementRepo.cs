using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageExpertiseManagementRepo : Repository<GarageExpertiseManagement>, IGarageExpertiseManagementRepo
    {
        public GarageExpertiseManagementRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
