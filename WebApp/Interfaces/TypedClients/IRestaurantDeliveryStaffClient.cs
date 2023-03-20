using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantDeliveryStaffClient
    {
        Task<IEnumerable<RestaurantDeliveryStaffDTO>> GetAllRestaurantDeliveryStaffsAsync(long restaurantId);
        Task<IEnumerable<RestaurantDeliveryStaffDTO>> GetAllRestaurantDeliveryStaffsAsync();
        Task<RestaurantDeliveryStaffDTO> GetRestaurantDeliveryStaffByIdAsync(long RestaurantDeliveryStaffId);
        Task<RestaurantDeliveryStaffDTO> AddRestaurantDeliveryStaffAsync(RestaurantDeliveryStaffDTO Entity);
        Task<RestaurantDeliveryStaffDTO> UpdateRestaurantDeliveryStaffAsync(RestaurantDeliveryStaffDTO Entity);
        Task DeleteRestaurantDeliveryStaffAsync(long RestaurantDeliveryStaffId);
        Task<RestaurantDeliveryStaffDTO> ToggleActiveStatus(long RestaurantDeliveryStaffId);
    }
}
