using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartsDealerClient
    {
        Task<SparePartsDealerDTO> GetSparePartsDealerByID(long Id);
        Task<IEnumerable<SparePartsDealerDTO>> GetSparePartsDealers();
        Task<SparePartsDealerDTO> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "");
        Task<SparePartsDealerDTO> Delete(long Id);
        Task<SparePartsDealerRegisterDTO> Update(SparePartsDealerRegisterDTO Entity);
        Task<string> UpdateProfilePicture(long SparePartsDealerId, string Path);
        Task<SparePartsDealerDTO> GetSparePartsDealerByUser();
        Task<object> UpdateSparePartsDealer(SparePartsDealerDTO model);
        Task<string> UpdateTheme(long SparePartsDealerId, string Theme);
        Task<string> UpdateThumbnail(long SparePartsDealerId, string Path);
        Task<string> UpdateSecondaryLogo(long SparePartsDealerId, string Path);
    }
}
