using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartBannerSettingService: ISparePartBannerSettingService
    {
        private readonly ISparePartBannerSettingRepo _repo;
        public SparePartBannerSettingService(ISparePartBannerSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartBannerSetting>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartBannerSetting>> GetSparePartBannerSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartBannerSetting>> GetSparePartBannerSettingBySparePartDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartsDealer");
        }

        public async Task<SparePartBannerSetting> AddSparePartBannerSettingAsync(SparePartBannerSetting Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartBannerSetting> UpdateSparePartBannerSettingAsync(SparePartBannerSetting Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartBannerSetting> ArchiveSparePartBannerSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
