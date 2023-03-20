using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
namespace WebAPI.Repositories.Domains
{
    public class ClientModulePurchasesRepo : Repository<ClientModulePurchases>, IClientModulePurchasesRepo
    {
        public ClientModulePurchasesRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
