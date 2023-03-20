using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class AggregatorService : IAggregatorService
    {
        private readonly IAggregatorRepo _repo;
        public AggregatorService(IAggregatorRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Aggregator>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<Aggregator>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "RestaurantBranch ");
		}
        public async Task<IEnumerable<Aggregator>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant ,RestaurantBranch ");
        }
        public async Task<IEnumerable<Aggregator>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "Restaurant ,RestaurantBranch ");
        }
        public async Task<Aggregator> AddAggregator(Aggregator Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<Aggregator> UpdateAggregator(Aggregator Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<Aggregator> ArchiveAggregator(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
