using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierServices : ISupplierService
    {
        private readonly ISupplierRepo _repo;

        public SupplierServices(ISupplierRepo repo)
        {
            _repo = repo;
        }

        public async Task<Supplier> AddSupplierAsync(Supplier Model)
        {
            return await _repo.InsertAsync(Model);
        }


        public async Task<Supplier> ArchiveSupplierAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<long> GetAllSuppliersCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _repo.GetAllAsync(x => x.Status == "Active");
        }
        public async Task<IEnumerable<Supplier>> GetAllForListAsync()
        {
            return await _repo.GetAllAsync( x => x.Status == "Active" || x.Status == "Inactive" || x.Status == "Approved");
        }
        public async Task<IEnumerable<Supplier>> GetSupplierForDropDownAssignAsync()
        {
            return await _repo.GetByIdAsync(x => x.SupplierCode != null);
        }
        public async Task<IEnumerable<Supplier>> GetSupplierForDropDownAsync()
        {
            return await _repo.GetByIdAsync(x => x.SupplierCode == null);
        }
        public async Task<IEnumerable<Supplier>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects : "SupplierDocuments");
        }
        public async Task<IEnumerable<Supplier>> GetAllForApproval()
        {
            return await _repo.GetAllAsync(x => x.Status == "Processing", ChildObjects: "SupplierDocuments");
        }
        public async Task<IEnumerable<Supplier>> GetByUserIdAsync(string UserId)
        {
            return await _repo.GetByIdAsync(x => x.UserId == UserId, "SupplierDocuments");
        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier Model)
        {
            return await _repo.UpdateAsync(Model);
        }



    }
}
