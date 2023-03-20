using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface IVendorClient
    {
        Task<IEnumerable<VendorDTO>> GetVendors();
        Task<VendorDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "");
        Task<VendorDTO> Delete(long Id);
        Task<object> Edit(VendorViewModel model);
        Task<object> Create(VendorViewModel model);
        Task<VendorDTO> GetVendorByID(long Id);
    }
}
