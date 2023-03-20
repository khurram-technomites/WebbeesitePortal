using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CouponCategoryRepo : Repository<CouponCategory>, ICouponCategoryRepo
    {
        public CouponCategoryRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }

    }
}
