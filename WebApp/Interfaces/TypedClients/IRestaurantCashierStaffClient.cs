using HelperClasses.DTOs.RestaurantCashierStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantCashierStaffClient
    {
        Task<IEnumerable<RestaurantCashierStaffDTO>> GetAllRestaurantCashierStaffsAsync(long restaurantId);
        Task<IEnumerable<RestaurantCashierStaffDTO>> GetAllRestaurantCashierStaffsAsync();
        Task<RestaurantCashierStaffDTO> GetRestaurantCashierStaffByIdAsync(long RestaurantCashierStaffId);
        Task<RestaurantCashierStaffDTO> AddRestaurantCashierStaffAsync(RestaurantCashierStaffDTO Entity);
        Task<RestaurantCashierStaffDTO> UpdateRestaurantCashierStaffAsync(RestaurantCashierStaffDTO Entity);
        Task DeleteRestaurantCashierStaffAsync(long RestaurantCashierStaffId);
        Task<RestaurantCashierStaffDTO> ToggleActiveStatus(long RestaurantCashierStaffId);
    }
}
