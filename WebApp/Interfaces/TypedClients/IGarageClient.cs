using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageClient
    {
        Task<GarageDTO> GetGarageByID(long Id);
        Task<GarageDTO> GetGarageByUser();

        Task<IEnumerable<GarageDTO>> GetGarageByVendor(long VendorId);
        Task<string> UpdateProfilePicture(long GarageId, string Path);
        Task<string> UpdateTheme(long GarageId, string Theme);
        Task<string> UpdateThumbnail(long GarageId, string Path);
        Task<string> UpdateSecondaryLogo(long GarageId, string Path);
        Task<object> Add(GarageRegisterDTO model);
        Task<object> Edit(GarageRegisterDTO model);
        Task<IEnumerable<GarageDTO>> GetGarages();
        Task<GarageDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "");
        Task<GarageDTO> Delete(long Id);
        Task<object> UpdateGarage(GarageDTO model);
        Task<object> UpdateVendorGarage(GarageDTO model);
        Task<object> Paid(long orderId, string paymentId);
    }
}
