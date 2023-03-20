using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantPrinterSettingService
    {
        Task<IEnumerable<RestaurantPrinterSetting>> GetAllAsync();
        Task<IEnumerable<RestaurantPrinterSetting>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantPrinterSetting>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantPrinterSetting>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantPrinterSetting> GetByTypeAndRestaurantBranchIdAsync(long RestaurantBranchId, string Type);
        Task<RestaurantPrinterSetting> AddRestaurantPrinterSettingAsync(RestaurantPrinterSetting Model);
        Task<RestaurantPrinterSetting> UpdateRestaurantPrinterSettingAsync(RestaurantPrinterSetting Model);
        Task<RestaurantPrinterSetting> ArchiveRestaurantPrinterSettingAsync(long Id);
    }
}
