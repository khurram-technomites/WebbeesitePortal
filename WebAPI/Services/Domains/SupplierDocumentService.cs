using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierDocumentService : ISupplierDocumentService
    {
        private readonly ISupplierDocumentRepo _repo;
        public SupplierDocumentService(ISupplierDocumentRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierDocument> AddSupplierDocumentAsync(SupplierDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierDocument>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SupplierDocument>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<SupplierDocument> UpdateSupplierDocumentAsync(SupplierDocument Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<SupplierDocument>> GetAllBySupplierId(long SupplierId)
        {
            return await _repo.GetAllAsync(x => x.SupplierId == SupplierId);
        }

        public async Task DeleteSupplierDocumentAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SupplierDocument>> GetAllByDocumentPath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Path == Path);
        }
    }
}
