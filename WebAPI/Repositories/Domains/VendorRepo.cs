using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class VendorRepo:Repository<Vendor>,IVendorRepo
    {
        private new readonly FougitoContext _context;

        public VendorRepo(FougitoContext context, ILoggerManager _logger) : base(context, _logger)
        {
            _context = context;
        }
    }
}
