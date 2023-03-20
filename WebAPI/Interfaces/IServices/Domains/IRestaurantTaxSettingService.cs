using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantTaxSettingService
    {
        Task<IEnumerable<RestaurantTaxSetting>> GetAllAsync();
        Task<IEnumerable<RestaurantTaxSetting>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantTaxSetting>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantTaxSetting>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantTaxSetting> AddRestaurantTaxSettingAsync(RestaurantTaxSetting Model);
        Task<RestaurantTaxSetting> UpdateRestaurantTaxSettingAsync(RestaurantTaxSetting Model);
        Task<RestaurantTaxSetting> ArchiveRestaurantTaxSettingAsync(long Id);
    }
}
