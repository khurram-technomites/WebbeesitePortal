using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantKitchenManagerService
    {
        Task<IEnumerable<RestaurantKitchenManager>> GetAllAsync();
        Task<IEnumerable<RestaurantKitchenManager>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantKitchenManager>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantKitchenManager>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantKitchenManager> GetRestaurantKitchenManagerByUserAsync(string UserId);
        Task<RestaurantKitchenManager> AddRestaurantKitchenManagerAsync(RestaurantKitchenManager Model);
        Task<RestaurantKitchenManager> UpdateRestaurantKitchenManagerAsync(RestaurantKitchenManager Model);
        Task<RestaurantKitchenManager> ArchiveRestaurantKitchenManagerAsync(long Id);
    }
}
