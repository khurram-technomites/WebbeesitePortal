using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageBannerSettingService : IGarageBannerSettingService
    {
        private readonly IGarageBannerSettingRepo _repo;
        public GarageBannerSettingService(IGarageBannerSettingRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GarageBannerSetting>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageBannerSetting>> GetGarageBannerSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageBannerSetting>> GetGarageBannerSettingByGaragedIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageBannerSetting> AddGarageBannerSettingAsync(GarageBannerSetting Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageBannerSetting> UpdateGarageBannerSettingAsync(GarageBannerSetting Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageBannerSetting> ArchiveGarageBannerSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
