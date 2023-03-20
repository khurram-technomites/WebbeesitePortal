using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
namespace WebAPI.Repositories.Domains
{
    public class ClientModulePurchaseTransactionsRepo : Repository<ClientModulePurchaseTransactions>, IClientModulePurchaseTransactionsRepo
    {
        public ClientModulePurchaseTransactionsRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
