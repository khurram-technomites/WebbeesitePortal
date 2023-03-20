using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantBannerSettingService : IRestaurantBannerSettingService
    {
        private readonly IRestaurantBannerSettingRepo _repo;
        public RestaurantBannerSettingService(IRestaurantBannerSettingRepo repo)
        {
            _repo = repo;
        }
        public async Task<RestaurantBannerSetting> AddRestaurantBannerSettingAsync(RestaurantBannerSetting Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task ArchiveRestaurantBannerSettingAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task DeleteRestaurantBannerSettingAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<RestaurantBannerSetting>> GetAllAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<RestaurantBannerSetting>> GetBannerByType(long RestaurantId , string Type)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == RestaurantId && x.BannerType == Type);
        }

        public async Task<IEnumerable<RestaurantBannerSetting>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<RestaurantBannerSetting> UpdateRestaurantBannerSettingAsync(RestaurantBannerSetting Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
