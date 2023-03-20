using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantContentManagementService
    {
        Task<IEnumerable<RestaurantContentManagement>> GetAllAsync();
        Task<IEnumerable<RestaurantContentManagement>> GetRestaurantContentManagementByIdAsync(long Id);
        Task<IEnumerable<RestaurantContentManagement>> GetRestaurantContentManagementByRestaurantIdAsync(long RestaurantId);
        Task<RestaurantContentManagement> AddRestaurantContentManagementAsync(RestaurantContentManagement Entity);
        Task<RestaurantContentManagement> UpdateRestaurantContentManagementAsync(RestaurantContentManagement Entity);
    }
}
