using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierDocumentService
    {
        Task<IEnumerable<SupplierDocument>> GetAllAsync();
        Task<IEnumerable<SupplierDocument>> GetByIdAsync(long Id);
        Task<IEnumerable<SupplierDocument>> GetAllBySupplierId(long SupplierId);
        Task<IEnumerable<SupplierDocument>> GetAllByDocumentPath(string Path);
        Task<SupplierDocument> AddSupplierDocumentAsync(SupplierDocument Model);
        Task<SupplierDocument> UpdateSupplierDocumentAsync(SupplierDocument Model);
        Task DeleteSupplierDocumentAsync(long Id);
    }
}
