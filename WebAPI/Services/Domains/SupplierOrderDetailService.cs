using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierOrderDetailService : ISupplierOrderDetailService
    {
        private readonly ISupplierOrderDetailRepo _repo;
        public SupplierOrderDetailService(ISupplierOrderDetailRepo repo)
        {
            _repo = repo;
        }
        public async Task<SupplierOrderDetail> AddSupplierOrderDetailAsync(SupplierOrderDetail Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierOrderDetail>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SupplierOrderDetail>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<SupplierOrderDetail>> GetAllBySupplierOrderId(long SupplierOrderId)
        {
            return await _repo.GetAllAsync(x => x.SupplierOrderId == SupplierOrderId);
        }
        public async Task<IEnumerable<SupplierOrderDetail>> GetAllBySupplierItemId(long SupplierItemId)
        {
            return await _repo.GetAllAsync(x => x.SupplierItemId == SupplierItemId);
        }

        public async Task<SupplierOrderDetail> UpdateSupplierOrderDetailAsync(SupplierOrderDetail Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<SupplierOrderDetail> ArchiveSupplierOrderDetailAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
