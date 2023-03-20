using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantBranchService
    {
        Task<long> GetAllResaturantBranchesCountAsync(long RestaurantId);
        Task<IEnumerable<RestaurantBranch>> GetRestaurantBranchesBySlug(string slug);
        Task<IEnumerable<RestaurantBranch>> GetAllBranchesByRestaurant(long restaurantId);
        Task<IEnumerable<RestaurantBranch>> GetAllActiveBranchesByRestaurant(long restaurantId);
        Task<RestaurantBranch> AddRestaurantBranchAsync(RestaurantBranch Model);
        Task<IEnumerable<RestaurantBranch>> GetRestaurantBranchById(long id);
        Task<RestaurantBranch> UpdateRestaurantBranchAsync(RestaurantBranch Model);
        Task<RestaurantBranch> ArchiveRestaurantBranchAsync(long Id);
        Task<IEnumerable<RestaurantBranch>> GetBranchByName(string Name, long restaurantId, long id = 0);

    }
}
