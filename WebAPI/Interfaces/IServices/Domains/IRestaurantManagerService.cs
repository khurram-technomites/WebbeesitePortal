using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantManagerService
    {
        Task<IEnumerable<RestaurantManager>> GetAllAsync();
        Task<IEnumerable<RestaurantManager>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantManager>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantManager>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantManager> AddRestaurantManagerAsync(RestaurantManager Model);
        Task<RestaurantManager> UpdateRestaurantManagerAsync(RestaurantManager Model);
        Task<RestaurantManager> ArchiveRestaurantManagerAsync(long Id);
    }
}
