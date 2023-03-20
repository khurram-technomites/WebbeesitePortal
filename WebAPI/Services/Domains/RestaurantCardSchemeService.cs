using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantCardSchemeService : IRestaurantCardSchemeService
    {
        private readonly IRestaurantCardSchemeRepo _repo;
        public RestaurantCardSchemeService(IRestaurantCardSchemeRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantCardScheme> AddRestaurantCardSchemeAsync(RestaurantCardScheme Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantCardScheme> ArchiveRestaurantCardSchemeAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantCardScheme> UpdateRestaurantCardSchemeAsync(RestaurantCardScheme Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantCardScheme>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantCardScheme>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<RestaurantCardScheme>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant");
        }
        public async Task<IEnumerable<RestaurantCardScheme>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch");
        }
    }
}
