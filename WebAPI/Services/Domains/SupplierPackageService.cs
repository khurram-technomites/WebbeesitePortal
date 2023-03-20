using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierPackageService : ISupplierPackageService
    {
        private readonly ISupplierPackageRepo _repo;

        public SupplierPackageService(ISupplierPackageRepo repo)
        {
            _repo = repo;
        }


        public async Task<SupplierPackage> ArchiveSupplierPackageAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SupplierPackage>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SupplierPackage>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<SupplierPackage> UpdateSupplierPackageAsync(SupplierPackage Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<SupplierPackage> AddSupplierPackageAsync(SupplierPackage Model)
        {
            return await _repo.InsertAsync(Model);
        }
    }
}
