using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierCouponCategoryClient
    {
        Task<SupplierCouponCategoryDTO> GetSupplierCouponCategoryById(long Id);
        Task<IEnumerable<SupplierCouponCategoryDTO>> GetCouponCategoriesByCoupon(long CouponId);
        Task<IEnumerable<SupplierCouponCategoryDTO>> GetAllCouponCategories();
        Task<SupplierCouponCategoryDTO> GetSupplierCouponCategoryByCouponAndCategory(long CouponId, long CategoryId);
        Task<SupplierCouponCategoryDTO> AddSupplierCouponCategoryAsync(SupplierCouponCategoryDTO Entity);
        Task<SupplierCouponCategoryDTO> UpdateSupplierCouponCategoryAsync(SupplierCouponCategoryDTO Entity);
        Task DeleteSupplierCouponCategoryAsync(long SupplierCouponCategoryId);
    }
}
