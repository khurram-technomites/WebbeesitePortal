using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
namespace WebAPI.Repositories.Domains
{
    public class ModulePurchaseDetailsRepo : Repository<ModulePurchaseDetails>, IModulePurchaseDetailsRepo
    {
        public ModulePurchaseDetailsRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
