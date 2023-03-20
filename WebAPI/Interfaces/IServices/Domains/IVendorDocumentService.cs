using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IVendorDocumentService
    {
        Task<IEnumerable<VendorDocument>> GetByVendor(long VendorId);
        Task<IEnumerable<VendorDocument>> GetByID(long Id);
        Task<VendorDocument> AddDocument(VendorDocument Model);
        Task DeleteRecord(long Id);
    }
}
