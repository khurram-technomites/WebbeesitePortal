using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICouponCategoryService
    {
        Task<IEnumerable<CouponCategory>> GetCouponCategories();
        Task<IEnumerable<CouponCategory>> GetCouponCategoriesByCoupon(long CouponsID);
        Task<IEnumerable<CouponCategory>> GetCouponCategory(long id);
        Task<IEnumerable<CouponCategory>> GetCouponCategoryByCouponAndCategoryId(long CouponId, long CategoryId);
        Task<CouponCategory> AddCouponCategoryAsync(CouponCategory Model);
        Task<CouponCategory> UpdateCouponCategoryAsync(CouponCategory Model);
        Task ArchiveCouponCategoryAsync(long Id);
    }
}
