using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantBranchService : IRestaurantBranchService
    {
        private readonly IRestaurantBranchRepo _repo;

        public RestaurantBranchService(IRestaurantBranchRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantBranch> AddRestaurantBranchAsync(RestaurantBranch Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantBranch> ArchiveRestaurantBranchAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<RestaurantBranch>> GetAllBranchesByRestaurant(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<RestaurantBranch>> GetRestaurantBranchById(long id)
        {
            return await _repo.GetByIdAsync(x => x.Id == id, ChildObjects: "Restaurant, RestaurantServiceStaffs, RestaurantCashierStaffs");
        }

        public async Task<IEnumerable<RestaurantBranch>> GetBranchByName(string Name, long restaurantId, long id = 0)
        {
            var result = await _repo.GetAllAsync(x => x.NameAsPerTradeLicense == Name && x.RestaurantId == restaurantId && x.Id != id);
            return result;
        }

        public Task<IEnumerable<RestaurantBranch>> GetRestaurantBranchesBySlug(string slug)
        {
            return _repo.GetByIdAsync(x => x.Slug == slug, "BranchSchedules, Restaurant, Restaurant.RestaurantRatings, Restaurant.RestaurantImages, Restaurant.RestaurantRatings.User");
        }

        public async Task<RestaurantBranch> UpdateRestaurantBranchAsync(RestaurantBranch Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<long> GetAllResaturantBranchesCountAsync(long RestaurantId)
        {
            return await _repo.GetCount(x => x.RestaurantId == RestaurantId);
        }

        public async Task<IEnumerable<RestaurantBranch>> GetAllActiveBranchesByRestaurant(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Status == Enum.GetName(typeof(Status), Status.Active), "Restaurant");
        }
    }
}
