using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierCouponService
    {
        Task<long> GetAllCouponsCountAsync(long SupplierId = 0);
        Task<IEnumerable<SupplierCoupon>> GetAllAsync(long restaurantId = 0);
        Task<IEnumerable<SupplierCoupon>> GetAllAdminAsync();
        Task<IEnumerable<SupplierCoupon>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierCoupon>> GetByCustomerAsync(long RestaurantId, long CustomerId);
        Task<IEnumerable<SupplierCoupon>> GetByCodeAsync(string Code);
        Task<SupplierCoupon> AddCouponAsync(SupplierCoupon Model);
        Task<SupplierCoupon> UpdateCouponAsync(SupplierCoupon Model);
        Task<SupplierCoupon> ArchiveCouponAsync(long Id);
    }
}
