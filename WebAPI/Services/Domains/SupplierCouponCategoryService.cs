using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierCouponCategoryService : ISupplierCouponCategoryService
    {
        private readonly ISupplierCouponCategoryRepo _repo;
        public SupplierCouponCategoryService(ISupplierCouponCategoryRepo SupplierCouponCategoryRepo)
        {
            _repo = SupplierCouponCategoryRepo;
        }
        public async Task<SupplierCouponCategory> AddSupplierCouponCategoryAsync(SupplierCouponCategory Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task ArchiveSupplierCouponCategoryAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SupplierCouponCategory>> GetCouponCategories()
        {
            return await _repo.GetAllAsync(ChildObjects: "SupplierItemCategory");
        }

        public async Task<IEnumerable<SupplierCouponCategory>> GetCouponCategoriesByCoupon(long CouponsID)
        {
            return await _repo.GetAllAsync(x => x.SupplierCouponId == CouponsID, ChildObjects: "SupplierItemCategory");
        }

        public async Task<IEnumerable<SupplierCouponCategory>> GetSupplierCouponCategory(long id)
        {
            return await _repo.GetByIdAsync(x => x.Id == id, ChildObjects: "SupplierItemCategory");
        }

        public async Task<IEnumerable<SupplierCouponCategory>> GetSupplierCouponCategoryByCouponAndCategoryId(long CouponId, long CategoryId)
        {
            return await _repo.GetByIdAsync(x => x.SupplierCouponId == CouponId && x.SupplierItemCategoryId == CategoryId, ChildObjects: "SupplierItemCategory");
        }

        public async Task<SupplierCouponCategory> UpdateSupplierCouponCategoryAsync(SupplierCouponCategory Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
