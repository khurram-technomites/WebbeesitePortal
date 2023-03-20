using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageMenuManagementRepo:Repository<GarageMenuManagement>, IGarageMenuManagementRepo
    {
        public GarageMenuManagementRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
