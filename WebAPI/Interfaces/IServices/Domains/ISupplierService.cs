using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<long> GetAllSuppliersCountAsync();
        Task<IEnumerable<Supplier>> GetAllForListAsync();
        Task<IEnumerable<Supplier>> GetSupplierForDropDownAssignAsync();
        Task<IEnumerable<Supplier>> GetSupplierForDropDownAsync();
        Task<IEnumerable<Supplier>> GetByUserIdAsync(string UserId);
        Task<IEnumerable<Supplier>> GetByIdAsync(long Id);
        Task<IEnumerable<Supplier>> GetAllForApproval();
        Task<Supplier> AddSupplierAsync(Supplier Model);
        Task<Supplier> UpdateSupplierAsync(Supplier Model);
        Task<Supplier> ArchiveSupplierAsync(long Id);


    }
}
