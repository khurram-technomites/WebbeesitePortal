using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantAggregatorService : IRestaurantAggregatorService
    {
        private readonly IRestaurantAggregatorRepo _repo;
        public RestaurantAggregatorService(IRestaurantAggregatorRepo repo)
        {
            _repo = repo;
        }
        public async Task<RestaurantAggregator> AddRestaurantAggregatorAsync(RestaurantAggregator Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantAggregator> ArchiveRestaurantAggregatorAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantAggregator> UpdateRestaurantAggregatorAsync(RestaurantAggregator Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantAggregator>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantAggregator>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<RestaurantAggregator>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant");
        }
        public async Task<IEnumerable<RestaurantAggregator>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch");
        }
    }
}
