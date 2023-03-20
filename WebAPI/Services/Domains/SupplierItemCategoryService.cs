using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierItemCategoryService : ISupplierItemCategoryService
    {
        private readonly ISupplierItemCategoryRepo _repo;
        public SupplierItemCategoryService(ISupplierItemCategoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierItemCategory> AddSupplierItemCategoryAsync(SupplierItemCategory Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierItemCategory>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SupplierItemCategory>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<long> GetAllSupplierCategoriesCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<SupplierItemCategory> UpdateSupplierItemCategoryAsync(SupplierItemCategory Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<SupplierItemCategory> ArchiveSupplierItemCategoryAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
