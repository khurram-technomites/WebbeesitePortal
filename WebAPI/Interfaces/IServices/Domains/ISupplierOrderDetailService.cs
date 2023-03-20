using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierOrderDetailService
    {
        Task<IEnumerable<SupplierOrderDetail>> GetAllAsync();
        Task<IEnumerable<SupplierOrderDetail>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierOrderDetail>> GetAllBySupplierItemId(long SupplierItemId);
        Task<IEnumerable<SupplierOrderDetail>> GetAllBySupplierOrderId(long SupplierOrderId);
        Task<SupplierOrderDetail> AddSupplierOrderDetailAsync(SupplierOrderDetail Model);
        Task<SupplierOrderDetail> UpdateSupplierOrderDetailAsync(SupplierOrderDetail Model);
        Task<SupplierOrderDetail> ArchiveSupplierOrderDetailAsync(long Id);
    }
}
