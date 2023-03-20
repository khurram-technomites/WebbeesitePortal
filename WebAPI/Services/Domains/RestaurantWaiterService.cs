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
    public class RestaurantWaiterService : IRestaurantWaiterService
    {
        private readonly IRestaurantWaiterRepo _repo;

        public RestaurantWaiterService(IRestaurantWaiterRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantWaiter> AddRestaurantWaiterAsync(RestaurantWaiter Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantWaiter> ArchiveRestaurantWaiterAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantWaiter> UpdateRestaurantWaiterAsync(RestaurantWaiter Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantWaiter>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantWaiter>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<RestaurantWaiter>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant,RestaurantBranch");
        }
        public async Task<IEnumerable<RestaurantWaiter>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "RestaurantBranch");
        }
    }
}
