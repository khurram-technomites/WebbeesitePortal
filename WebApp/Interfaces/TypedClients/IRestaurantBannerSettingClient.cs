using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantBannerSettingClient
    {
        Task<IEnumerable<RestaurantBannerSettingDTO>> GetAllRestaurantBannerSettingsAsync(long RestaurantId);
        Task<RestaurantBannerSettingDTO> GetRestaurantBannerSettingByIdAsync(long RestaurantBannerSettingId);
        Task<IEnumerable<RestaurantBannerSettingDTO>> GetBannerByType(long RestaurantId , string Type);
        Task<RestaurantBannerSettingDTO> AddRestaurantBannerSettingAsync(RestaurantBannerSettingDTO Entity);
        Task<RestaurantBannerSettingDTO> UpdateRestaurantBannerSettingAsync(RestaurantBannerSettingDTO Entity);
        Task<RestaurantBannerSettingDTO> UpdateRestaurantBannerSettingMenuImage(long Id);
        Task DeleteRestaurantBannerSettingAsync(long RestaurantBannerSettingId);
        Task<RestaurantBannerSettingDTO> ToggleActiveStatus(long RestaurantBannerSettingId);
    }
}
