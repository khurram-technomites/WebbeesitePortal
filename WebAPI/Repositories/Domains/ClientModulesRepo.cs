using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
namespace WebAPI.Repositories.Domains
{
    public class ClientModulesRepo : Repository<ClientModules>, IClientModulesRepo
    {
        public ClientModulesRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
