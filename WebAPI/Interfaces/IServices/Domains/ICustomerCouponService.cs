using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerCouponService
    {
        Task<IEnumerable<CustomerCoupon>> GetCoupon(long CustomerID, long CouponsID);
        Task<IEnumerable<CustomerCoupon>> GetCouponsByCouponID(long CouponsID);
        Task<IEnumerable<Coupon>> GetCustomerCoupons(long CustometId);
        Task<CustomerCoupon> AddCustomerCouponAsync(CustomerCoupon Model);
        Task<CustomerCoupon> UpdateCustomerCouponAsync(CustomerCoupon Model);
        //Task<CustomerCoupon> ArchiveCustomerCouponAsync(long Id); UZAIF
        Task DeleteCustomerCouponAsync(long Id);
    }
}
