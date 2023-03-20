using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class MenuItemOptionValueRepo : Repository<MenuItemOptionValue>, IMenuItemOptionValueRepo
    {
        public MenuItemOptionValueRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
