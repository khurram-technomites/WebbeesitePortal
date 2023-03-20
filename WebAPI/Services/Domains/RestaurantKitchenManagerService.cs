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
    public class RestaurantKitchenManagerService : IRestaurantKitchenManagerService
    {
        private readonly IRestaurantKitchenManagerRepo _repo;
        public RestaurantKitchenManagerService(IRestaurantKitchenManagerRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<RestaurantKitchenManager>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<RestaurantKitchenManager>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<RestaurantKitchenManager>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant,RestaurantBranch");
        }
        public async Task<IEnumerable<RestaurantKitchenManager>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "RestaurantBranch");
        }
        public async Task<RestaurantKitchenManager> GetRestaurantKitchenManagerByUserAsync(string UserId)
        {
            IEnumerable<RestaurantKitchenManager> list = await _repo.GetByIdAsync(x => x.UserId == UserId);
            return list.FirstOrDefault();
        }

        public async Task<RestaurantKitchenManager> AddRestaurantKitchenManagerAsync(RestaurantKitchenManager Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<RestaurantKitchenManager> UpdateRestaurantKitchenManagerAsync(RestaurantKitchenManager Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<RestaurantKitchenManager> ArchiveRestaurantKitchenManagerAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
