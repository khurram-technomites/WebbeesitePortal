
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{

    public class CouponCategoryService : ICouponCategoryService
    {
        private readonly ICouponCategoryRepo _repo;
        public CouponCategoryService(ICouponCategoryRepo couponCategoryRepo)
        {
            _repo = couponCategoryRepo;
        }
        public async Task<CouponCategory> AddCouponCategoryAsync(CouponCategory Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task ArchiveCouponCategoryAsync(long Id)
        {
             await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<CouponCategory>> GetCouponCategories()
        {
            return await _repo.GetAllAsync(ChildObjects : "Category");
        }

        public async Task<IEnumerable<CouponCategory>> GetCouponCategoriesByCoupon(long CouponsID)
        {
            return await _repo.GetAllAsync(x=>x.CouponId == CouponsID , ChildObjects: "Category");
        }

        public async Task<IEnumerable<CouponCategory>> GetCouponCategory(long id)
        {
            return await _repo.GetByIdAsync(x => x.Id == id , ChildObjects: "Category");
        }

        public async Task<IEnumerable<CouponCategory>> GetCouponCategoryByCouponAndCategoryId(long CouponId , long CategoryId)
        {
            return await _repo.GetByIdAsync(x => x.CouponId == CouponId && x.CategoryId == CategoryId, ChildObjects: "Category");
        }

        public async Task<CouponCategory> UpdateCouponCategoryAsync(CouponCategory Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
