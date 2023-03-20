using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IAggregatorService
    {
        Task<IEnumerable<Aggregator>> GetAllAsync();
        Task<IEnumerable<Aggregator>> GetByIdAsync(long Id);
        Task<IEnumerable<Aggregator>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<Aggregator>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<Aggregator> AddAggregator(Aggregator Model);
        Task<Aggregator> UpdateAggregator(Aggregator Model);
        Task<Aggregator> ArchiveAggregator(long Id);


    }
}
