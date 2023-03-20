using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantAggregatorService
    {
        Task<IEnumerable<RestaurantAggregator>> GetAllAsync();
        Task<IEnumerable<RestaurantAggregator>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantAggregator>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantAggregator>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantAggregator> AddRestaurantAggregatorAsync(RestaurantAggregator Model);
        Task<RestaurantAggregator> UpdateRestaurantAggregatorAsync(RestaurantAggregator Model);
        Task<RestaurantAggregator> ArchiveRestaurantAggregatorAsync(long Id);
    }
}
