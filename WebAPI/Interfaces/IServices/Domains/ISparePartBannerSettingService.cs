using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartBannerSettingService
    {
        Task<IEnumerable<SparePartBannerSetting>> GetAllAsync();
        Task<IEnumerable<SparePartBannerSetting>> GetSparePartBannerSettingByIdAsync(long Id);
        Task<IEnumerable<SparePartBannerSetting>> GetSparePartBannerSettingBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartBannerSetting> AddSparePartBannerSettingAsync(SparePartBannerSetting Model);
        Task<SparePartBannerSetting> UpdateSparePartBannerSettingAsync(SparePartBannerSetting Model);
        Task<SparePartBannerSetting> ArchiveSparePartBannerSettingAsync(long Id);
    }
}
