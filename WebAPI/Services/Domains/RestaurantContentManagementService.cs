using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantContentManagementService : IRestaurantContentManagementService
    {
        private readonly IRestaurantContentManagementRepo _repo;

        public RestaurantContentManagementService(IRestaurantContentManagementRepo repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<RestaurantContentManagement>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public async Task<RestaurantContentManagement> AddRestaurantContentManagementAsync(RestaurantContentManagement Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<IEnumerable<RestaurantContentManagement>> GetRestaurantContentManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<RestaurantContentManagement>> GetRestaurantContentManagementByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId);
        }

        public async Task<RestaurantContentManagement> UpdateRestaurantContentManagementAsync(RestaurantContentManagement Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
