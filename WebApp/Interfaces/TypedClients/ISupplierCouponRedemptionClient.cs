using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierCouponRedemptionClient
    {
        Task<IEnumerable<SupplierCouponRedemptionDTO>> GetSupplierCouponRedemptionsByRestaurant(string RestaurantId , long SupplierCouponId );
        Task<IEnumerable<SupplierCouponRedemptionDTO>> GetAllSupplierCouponRedemptions(long SupplierCouponId);
        Task<SupplierCouponRedemptionDTO> AddSupplierCouponRedemptionAsync(SupplierCouponRedemptionDTO Entity);
    }
}
