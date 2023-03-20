using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantTaxSettingClient
    {
        Task<IEnumerable<RestaurantTaxSettingDTO>> GetAllAsync();
        Task <RestaurantTaxSettingDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<RestaurantTaxSettingDTO>> GetAllByRestaurantIdAsync(long restaurantId);
        Task<RestaurantTaxSettingDTO> GetByRestaurantBranchIdAsync(long restaurantBranchId);
        Task<RestaurantTaxSettingDTO> AddRestaurantTaxSettingAsync(RestaurantTaxSettingDTO Entity);
        Task<RestaurantTaxSettingDTO> UpdateRestaurantTaxSettingAsync(RestaurantTaxSettingDTO Entity);
        Task DeleteRestaurantTaxSettingAsync(long Id);
    }
}
