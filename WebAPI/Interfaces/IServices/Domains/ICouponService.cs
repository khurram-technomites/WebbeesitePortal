using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICouponService
    {
        Task<long> GetAllCouponsCountAsync(long RestaurantId = 0);
        Task<IEnumerable<Coupon>> GetAllAsync(long restaurantId = 0);
        Task<IEnumerable<Coupon>> GetAllAdminAsync();
        Task<IEnumerable<Coupon>> GetByIdAsync(long Id);
        Task<IEnumerable<Coupon>> GetByCustomerAsync(long RestaurantId, long CustomerId);
        Task<IEnumerable<Coupon>> GetByCodeAsync(string Code);
        Task<Coupon> AddCouponAsync(Coupon Model);
        Task<Coupon> UpdateCouponAsync(Coupon Model);
        Task<Coupon> ArchiveCouponAsync(long Id);
        
    }
}
