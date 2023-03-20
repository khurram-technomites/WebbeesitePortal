using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierCouponRedemptionService
    {
        Task<SupplierCouponRedemption> AddSupplierCouponRedemption(SupplierCouponRedemption Model);
        Task<IEnumerable<SupplierCouponRedemption>> GetCouponRedemptionByRestaurant(long SupplierCouponId, string RestaurantId);
        Task<IEnumerable<SupplierCouponRedemption>> GetCouponRedemption(long SupplierCouponId);
        


        }
}
