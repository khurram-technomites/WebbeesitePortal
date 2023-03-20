using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierPackageService
    {
        Task<IEnumerable<SupplierPackage>> GetAllAsync();
        Task<IEnumerable<SupplierPackage>> GetByIdAsync(long Id);
        Task<SupplierPackage> AddSupplierPackageAsync(SupplierPackage Model);
        Task<SupplierPackage> UpdateSupplierPackageAsync(SupplierPackage Model);
        Task<SupplierPackage> ArchiveSupplierPackageAsync(long Id);
    }
}
