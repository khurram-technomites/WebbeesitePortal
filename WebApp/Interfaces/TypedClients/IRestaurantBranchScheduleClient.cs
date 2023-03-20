using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantBranchScheduleClient
    {
        Task<IEnumerable<RestaurantBranchScheduleDTO>> GetAllRestaurantBranchSchedulesAsync(long branchId);
        Task<RestaurantBranchScheduleDTO> GetRestaurantBranchScheduleByIdAsync(long id);
        Task<RestaurantBranchScheduleDTO> AddRestaurantBranchScheduleAsync(RestaurantBranchScheduleDTO Entity);
        Task<RestaurantBranchScheduleDTO> UpdateRestaurantBranchScheduleAsync(RestaurantBranchScheduleDTO Entity);
        Task DeleteRestaurantBranchScheduleAsync(long RestaurantBranchScheduleId);
    }
}
