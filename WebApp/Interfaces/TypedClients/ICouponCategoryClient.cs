using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICouponCategoryClient
    {
        Task<CouponCategoryDTO> GetCouponCategoryById(long Id);
        Task<IEnumerable<CouponCategoryDTO>> GetCouponCategoriesByCoupon(long CouponId);
        Task<IEnumerable<CouponCategoryDTO>> GetAllCouponCategories();
        Task<CouponCategoryDTO> GetCouponCategoryByCouponAndCategory(long CouponId, long CategoryId);
        Task<CouponCategoryDTO> AddCouponCategoryAsync(CouponCategoryDTO Entity);
        Task<CouponCategoryDTO> UpdateCouponCategoryAsync(CouponCategoryDTO Entity);
        Task DeleteCouponCategoryAsync(long CouponCategoryId);
    }
}
