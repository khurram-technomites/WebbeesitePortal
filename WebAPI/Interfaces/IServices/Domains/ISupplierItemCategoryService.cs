using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierItemCategoryService
    {
        Task<IEnumerable<SupplierItemCategory>> GetAllAsync();
        Task<IEnumerable<SupplierItemCategory>> GetByIdAsync(long Id);
        Task<long> GetAllSupplierCategoriesCountAsync();
        Task<SupplierItemCategory> AddSupplierItemCategoryAsync(SupplierItemCategory Model);
        Task<SupplierItemCategory> UpdateSupplierItemCategoryAsync(SupplierItemCategory Model);
        Task<SupplierItemCategory> ArchiveSupplierItemCategoryAsync(long Id);
    }
}
