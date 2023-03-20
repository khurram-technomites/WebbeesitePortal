using HelperClasses.DTOs.Supplier;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISupplierPackageClient
    {
        Task<IEnumerable<SupplierPackageDTO>> GetAllAsync();
        Task<IEnumerable<SupplierPackageDTO>> GetByIdAsync(long Id);
        Task<SupplierPackageDTO> AddSupplierPackageAsync(SupplierPackageDTO Model);
        Task<SupplierPackageDTO> UpdateSupplierPackageAsync(SupplierPackageDTO Model);
        Task<SupplierPackageDTO> ToggleActiveStatus(long Id);
        Task DeleteSupplierPackageAsync(long Id);
    }
}
