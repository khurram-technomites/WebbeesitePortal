using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierCouponClient
    {
        Task<IEnumerable<SupplierCouponDTO>> GetAllCouponsAsync(long RestaurantId = 0);
        Task<IEnumerable<SupplierCouponDTO>> GetAllAdminCouponsAsync();
        Task<SupplierCouponDTO> GetCouponByIdAsync(long CouponId);
        Task<SupplierCouponDTO> AddCouponAsync(SupplierCouponDTO Entity);
        Task<SupplierCouponDTO> UpdateCouponAsync(SupplierCouponDTO Entity);
        Task<SupplierCouponDTO> DeleteCouponAsync(long CouponId);
        Task<SupplierCouponDTO> ToggleActiveStatus(long CouponId);
    }
}
