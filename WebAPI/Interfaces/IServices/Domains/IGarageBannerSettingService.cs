using HelperClasses.DTOs.Garage.Filter;
using HelperClasses.DTOs.Garage;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageBannerSettingService
    {
        Task<IEnumerable<GarageBannerSetting>> GetAllAsync();
        Task<IEnumerable<GarageBannerSetting>> GetGarageBannerSettingByIdAsync(long Id);
        Task<IEnumerable<GarageBannerSetting>> GetGarageBannerSettingByGaragedIdAsync(long GaragedId);
        Task<GarageBannerSetting> AddGarageBannerSettingAsync(GarageBannerSetting Model);
        Task<GarageBannerSetting> UpdateGarageBannerSettingAsync(GarageBannerSetting Model);
        Task<GarageBannerSetting> ArchiveGarageBannerSettingAsync(long Id);
    }
}
