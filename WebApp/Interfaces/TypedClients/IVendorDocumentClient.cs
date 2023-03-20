using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;
namespace WebApp.Interfaces.TypedClients
{
    public interface IVendorDocumentClient
    {
        Task<IEnumerable<VendorDocumentViewModel>> GetAllByVendor(long VendorId);
        Task<VendorDocumentViewModel> AddVendorDocument(VendorDocumentDTO Model);
        Task Delete(long Id);
    }
}
