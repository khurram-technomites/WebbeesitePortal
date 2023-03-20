using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantTaxSettingService : IRestaurantTaxSettingService
    {
        private readonly IRestaurantTaxSettingRepo _repo;
        public RestaurantTaxSettingService(IRestaurantTaxSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<RestaurantTaxSetting> AddRestaurantTaxSettingAsync(RestaurantTaxSetting Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantTaxSetting> ArchiveRestaurantTaxSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantTaxSetting> UpdateRestaurantTaxSettingAsync(RestaurantTaxSetting Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantTaxSetting>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantTaxSetting>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<RestaurantTaxSetting>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant ,RestaurantBranch ");
        }
        public async Task<IEnumerable<RestaurantTaxSetting>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId, ChildObjects: "RestaurantBranch");
        }
    }
}
