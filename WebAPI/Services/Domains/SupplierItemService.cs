using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierItemService : ISupplierItemService
    {
        private readonly ISupplierItemRepo _repo;
        public SupplierItemService(ISupplierItemRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierItem> AddSupplierItemAsync(SupplierItem Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierItem>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<long> GetAllItemsCountBySupplierIdAsync(long supplierId)
        {
            return await _repo.GetCount(x => x.SupplierId == supplierId);
        }
        public async Task<IEnumerable<SupplierItem>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "SupplierItemImages");
        }

        public async Task<SupplierItem> UpdateSupplierItemAsync(SupplierItem Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<SupplierItem>> GetAllBySupplierId(long SupplierId, PagingParameters Model = null)
        {
            return await _repo.GetAllAsync(x => x.SupplierId == SupplierId, ChildObjects: "Category", Pagination: Model);
        }

        public async Task<IEnumerable<SupplierItem>> GetAllByCategoryId(long CategoryId)
        {
            return await _repo.GetAllAsync(x => x.CategoryId == CategoryId);
        }

        public async Task<IEnumerable<SupplierItem>> GetItemBySupplierAndCategoryID(long SupplierId, long CategoryId, PagingParameters Model = null)
        {
            return await _repo.GetAllAsync(x => x.SupplierId == SupplierId && x.CategoryId == CategoryId, Pagination: Model);
        }

        public async Task<SupplierItem> ArchiveSupplierItemAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
