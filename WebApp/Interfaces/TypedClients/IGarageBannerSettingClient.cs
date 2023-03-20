using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageBannerSettingClient
    {
        Task<IEnumerable<GarageBannerSettingDTO>> GetAllAsync();
        Task<GarageBannerSettingDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageBannerSettingDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageBannerSettingDTO> AddGarageBannerSettingAsync(GarageBannerSettingDTO Entity);
        Task<GarageBannerSettingDTO> UpdateGarageBannerSettingAsync(GarageBannerSettingDTO Entity);
        Task DeleteGarageBannerSettingAsync(long Id);
    }
}
