using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierClient
    {
        Task<IEnumerable<SupplierCardDTO>> GetAllSuppliersAsync();
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersListAsync();
        Task<IEnumerable<SupplierDTO>> GetSupplierByIDAsync(long Id);
        Task<SupplierDTO> AddSupplierAsync(SupplierDTO Model);
        Task<IEnumerable<SupplierDTO>> GetSupplierForDropDwonAssignAsync();
        Task<IEnumerable<SupplierDTO>> GetSupplierForDropDwonAsync();
        Task<IEnumerable<SupplierItemDTO>> GetAllSupplierItems(long SupplierId, PagingParameters paging);
        Task<IEnumerable<SupplierDTO>> GetAllForApproval();
        Task<SupplierDTO> UpdateSupplierAsync(SupplierDTO Model);
        Task<SupplierDTO> Reject(SupplierDTO Model);
        Task<SupplierDTO> Approve(SupplierDTO Model);
        Task<SupplierDTO> ToggleApproveAsync(long Id);
        Task<SupplierDTO> ToggleActiveStatusAsync(long Id);
        Task<IEnumerable<SupplierDTO>> GetSupplierById(long SupplierId);
        Task<SupplierDTO> DeleteSupplier(long id);
        Task<IEnumerable<SupplierItemCategoryDTO>> GetAllCategory(long SupplierId);
        Task<IEnumerable<SupplierItemDTO>> GetAllSupplierItemsByCategory(long SupplierId, long CategoryId, PagingParameters paging);
        Task<SupplierItemDTO> GetItemById(long SupplierItemId);

    }
}
