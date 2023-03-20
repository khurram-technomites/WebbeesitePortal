using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantCouponClient
    {
        Task<RestaurantCouponDTO> GetRestaurantCoupon(long RestaurantId, long CouponId);
        Task<IEnumerable<RestaurantCouponDTO>> GetRestaurantCouponsByCoupon(long CouponId);
        Task<IEnumerable<SupplierCouponDTO>> GetAllRestaurantCoupons(long RestaurantId);
        Task<RestaurantCouponDTO> AddRestaurantCouponAsync(RestaurantCouponDTO Entity);
        Task<RestaurantCouponDTO> UpdateRestaurantCouponAsync(RestaurantCouponDTO Entity);
        Task DeleteRestaurantCouponAsync(long RestaurantCouponId);
    }
}
