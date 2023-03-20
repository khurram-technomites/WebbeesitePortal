using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CustomerCouponRepo : Repository<CustomerCoupon>, ICustomerCouponRepo
    {
        public CustomerCouponRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }

    }
}
