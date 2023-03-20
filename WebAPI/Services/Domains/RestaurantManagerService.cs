using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantManagerService : IRestaurantManagerService
    {
        private readonly IRestaurantManagerRepo _repo;

        public RestaurantManagerService(IRestaurantManagerRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantManager> AddRestaurantManagerAsync(RestaurantManager Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantManager> ArchiveRestaurantManagerAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantManager> UpdateRestaurantManagerAsync(RestaurantManager Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantManager>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantManager>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "RestaurantBranch");
        }
        public async Task<IEnumerable<RestaurantManager>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant,RestaurantBranch");
        }
        public async Task<IEnumerable<RestaurantManager>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "RestaurantBranch");
        }
    }
}
