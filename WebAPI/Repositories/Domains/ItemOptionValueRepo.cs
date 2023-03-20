using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class ItemOptionValueRepo : Repository<ItemOptionValue>, IItemOptionValueRepo
    {
        public ItemOptionValueRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
