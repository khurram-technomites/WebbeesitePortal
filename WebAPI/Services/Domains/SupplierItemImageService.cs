using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class SupplierItemImageService : ISupplierItemImageService
    {
        private readonly ISupplierItemImageRepo _repo;
        public SupplierItemImageService(ISupplierItemImageRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierItemImage> AddSupplierItemImageAsync(SupplierItemImage Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierItemImage>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SupplierItemImage>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SupplierItemImage>> GetBySupplieridAsync(long SupplierId)
        {
            return await _repo.GetAllAsync(x => x.SupplierItemId == SupplierId);
        }

        public async Task<SupplierItemImage> UpdateSupplierItemImageAsync(SupplierItemImage Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SupplierItemImage> ArchiveSupplierItemImageAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SupplierItemImage>> GetByImagePathAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Path == Path);
        }

        public async Task DeleteSupplierItemImageAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
