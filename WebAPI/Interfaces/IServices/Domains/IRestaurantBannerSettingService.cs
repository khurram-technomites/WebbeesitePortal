using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantBannerSettingService
    {
        Task<IEnumerable<RestaurantBannerSetting>> GetAllAsync(long restaurantId);
        Task<IEnumerable<RestaurantBannerSetting>> GetByIdAsync(long Id);
        Task<IEnumerable<RestaurantBannerSetting>> GetBannerByType(long restaurantId , string Type);
        Task<RestaurantBannerSetting> AddRestaurantBannerSettingAsync(RestaurantBannerSetting Model);
        Task<RestaurantBannerSetting> UpdateRestaurantBannerSettingAsync(RestaurantBannerSetting Model);
        Task ArchiveRestaurantBannerSettingAsync(long Id);
        Task DeleteRestaurantBannerSettingAsync(long Id);
    }
}
