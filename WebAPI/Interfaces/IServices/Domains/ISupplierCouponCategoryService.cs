using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierCouponCategoryService
    {
        Task<IEnumerable<SupplierCouponCategory>> GetCouponCategories();
        Task<IEnumerable<SupplierCouponCategory>> GetCouponCategoriesByCoupon(long CouponsID);
        Task<IEnumerable<SupplierCouponCategory>> GetSupplierCouponCategory(long id);
        Task<IEnumerable<SupplierCouponCategory>> GetSupplierCouponCategoryByCouponAndCategoryId(long CouponId, long CategoryId);
        Task<SupplierCouponCategory> AddSupplierCouponCategoryAsync(SupplierCouponCategory Model);
        Task<SupplierCouponCategory> UpdateSupplierCouponCategoryAsync(SupplierCouponCategory Model);
        Task ArchiveSupplierCouponCategoryAsync(long Id);
    }
}
