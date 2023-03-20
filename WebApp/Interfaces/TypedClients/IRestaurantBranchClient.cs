using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantBranchClient
    {
        Task<IEnumerable<RestaurantBranchDTO>> GetAllRestaurantBranchsAsync(long RestaurantId);
        Task<RestaurantBranchDTO> GetRestaurantBranchByIdAsync(long RestaurantBranchId);
        Task<RestaurantBranchDTO> AddRestaurantBranchAsync(RestaurantBranchDTO Entity);
        Task<RestaurantBranchDTO> UpdateRestaurantBranchAsync(RestaurantBranchDTO Entity);
        Task DeleteRestaurantBranchAsync(long RestaurantBranchId);
        Task<RestaurantBranchDTO> ToggleActiveStatus(long RestaurantBranchId);
        Task<RestaurantBranchDTO> ToggleCloseStatus(long RestaurantBranchId, TimeSpan? ClosingTimeSpan);
        Task<RestaurantBranchDTO> ToggleMainStatus(long RestaurantBranchId);
    }
}
