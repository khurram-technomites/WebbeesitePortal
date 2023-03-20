using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebApp.ViewModels;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs.RestaurantKitchenManager;
using HelperClasses.DTOs.RestaurantCashierStaff;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantKitchenManagerClient
    {
        Task<IEnumerable<RestaurantKitchenManagerDTO>> GetAllAsync();
        Task<RestaurantKitchenManagerDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<RestaurantKitchenManagerDTO>> GetAllByRestaurantIdAsync(long restaurantId);
        Task<RestaurantKitchenManagerDTO> GetByRestaurantBranchIdAsync(long restaurantBranchId);
        Task<RestaurantKitchenManagerDTO> AddRestaurantKitchenManagerAsync(RestaurantKitchenManagerDTO Entity);
        Task<RestaurantKitchenManagerDTO> UpdateRestaurantKitchenManagerAsync(RestaurantKitchenManagerDTO Entity);
        Task<RestaurantKitchenManagerDTO> ToggleActiveStatus(long RestaurantKitchenManagerId);
        Task DeleteRestaurantKitchenManagerAsync(long Id);
    }
}
