using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierItemService
    {
        Task<IEnumerable<SupplierItem>> GetAllAsync();
        Task<long> GetAllItemsCountBySupplierIdAsync(long supplierId);
        Task<IEnumerable<SupplierItem>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierItem>> GetAllBySupplierId(long SupplierId, PagingParameters Model = null);
        Task<IEnumerable<SupplierItem>> GetItemBySupplierAndCategoryID(long SupplierId, long CategoryId, PagingParameters Model = null);
        Task<IEnumerable<SupplierItem>> GetAllByCategoryId(long CategoryId);
        Task<SupplierItem> AddSupplierItemAsync(SupplierItem Model);
        Task<SupplierItem> UpdateSupplierItemAsync(SupplierItem Model);
        Task<SupplierItem> ArchiveSupplierItemAsync(long Id);
    }
}
