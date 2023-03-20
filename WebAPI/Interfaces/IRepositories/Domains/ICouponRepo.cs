using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ICouponRepo : IRepository<Coupon>
    {
        Task<IEnumerable<Coupon>> GetCouponByCustomer(long RestaurantId, long CustomerId);
    }
}
