using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCouponService
    {
        Task<IEnumerable<RestaurantCoupon>> GetCoupon(long CustomerID, long CouponsID);
        Task<IEnumerable<RestaurantCoupon>> GetCouponsByCouponID(long CouponsID);
        Task<IEnumerable<SupplierCoupon>> GetRestaurantCoupons(long CustometId);
        Task<RestaurantCoupon> AddRestaurantCouponAsync(RestaurantCoupon Model);
        Task<RestaurantCoupon> UpdateRestaurantCouponAsync(RestaurantCoupon Model);
        Task DeleteRestaurantCouponAsync(long Id);
    }
}
