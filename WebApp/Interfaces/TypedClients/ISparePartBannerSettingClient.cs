using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartBannerSettingClient
    {
        Task<IEnumerable<SparePartBannerSettingDTO>> GetAllAsync();
        Task<SparePartBannerSettingDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartBannerSettingDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartBannerSettingDTO> AddSparePartBannerSettingAsync(SparePartBannerSettingDTO Entity);
        Task<SparePartBannerSettingDTO> UpdateSparePartBannerSettingAsync(SparePartBannerSettingDTO Entity);
        Task DeleteSparePartBannerSettingAsync(long Id);
    }
}
