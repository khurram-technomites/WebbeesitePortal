using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantWaiterClient
    {
        Task<IEnumerable<RestaurantWaiterDTO>> GetAllAsync();
        Task<RestaurantWaiterDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<RestaurantWaiterDTO>> GetAllByRestaurantIdAsync(long restaurantId);
        Task<RestaurantWaiterDTO> GetByRestaurantBranchIdAsync(long restaurantBranchId);
        Task<RestaurantWaiterDTO> AddRestaurantWaiterAsync(RestaurantWaiterDTO Entity);
        Task<RestaurantWaiterDTO> UpdateRestaurantWaiterAsync(RestaurantWaiterDTO Entity);
        Task<RestaurantWaiterDTO> ToggleActiveStatus(long RestaurantWaiterStaffId);
        Task DeleteRestaurantWaiterAsync(long Id);
    }
}
