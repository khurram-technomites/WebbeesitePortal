using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierItemImageService
    {
        Task<IEnumerable<SupplierItemImage>> GetAllAsync();
        Task<IEnumerable<SupplierItemImage>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierItemImage>> GetBySupplieridAsync(long Supplierid);
        Task<IEnumerable<SupplierItemImage>> GetByImagePathAsync(string Path);
        Task<SupplierItemImage> AddSupplierItemImageAsync(SupplierItemImage Model);
        Task<SupplierItemImage> UpdateSupplierItemImageAsync(SupplierItemImage Model);
        Task<SupplierItemImage> ArchiveSupplierItemImageAsync(long Id);
        Task DeleteSupplierItemImageAsync(long Id);

    }
}
