using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantWaiterService
    {
        Task<IEnumerable<RestaurantWaiter>> GetAllAsync();
        Task<IEnumerable<RestaurantWaiter>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantWaiter>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantWaiter>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantWaiter> AddRestaurantWaiterAsync(RestaurantWaiter Model);
        Task<RestaurantWaiter> UpdateRestaurantWaiterAsync(RestaurantWaiter Model);
        Task<RestaurantWaiter> ArchiveRestaurantWaiterAsync(long Id);
    }
}
