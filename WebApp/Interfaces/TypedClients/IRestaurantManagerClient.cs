using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantManagerClient
    {
        Task<IEnumerable<RestaurantManagerDTO>> GetAllAsync();
        Task<RestaurantManagerDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<RestaurantManagerDTO>> GetAllByRestaurantIdAsync(long restaurantId);
        Task<RestaurantManagerDTO> GetByRestaurantBranchIdAsync(long restaurantBranchId);
        Task<RestaurantManagerDTO> AddRestaurantManagerAsync(RestaurantManagerDTO Entity);
        Task<RestaurantManagerDTO> UpdateRestaurantManagerAsync(RestaurantManagerDTO Entity);
        Task<RestaurantManagerDTO> ToggleActiveStatus(long Id);
        Task DeleteRestaurantManagerAsync(long Id);
    }
}
