using WebAPI.Models;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;

namespace WebAPI.Repositories.Domains
{
    public class ModuleRepo : Repository<Module>, IModuleRepo
    {
        private new readonly FougitoContext _context;

        public ModuleRepo(FougitoContext context, ILoggerManager _logger) : base(context, _logger)
        {
            _context = context;
        }

    }
}
