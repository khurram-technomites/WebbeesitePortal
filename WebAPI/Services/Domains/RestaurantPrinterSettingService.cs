using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Mapper.RestaurantMapper;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantPrinterSettingService : IRestaurantPrinterSettingService
    {
        private readonly IRestaurantPrinterSettingRepo _repo;
        public RestaurantPrinterSettingService(IRestaurantPrinterSettingRepo repo)
        {
            _repo = repo;
        }
        public async Task<RestaurantPrinterSetting> AddRestaurantPrinterSettingAsync(RestaurantPrinterSetting Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantPrinterSetting> ArchiveRestaurantPrinterSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<RestaurantPrinterSetting> UpdateRestaurantPrinterSettingAsync(RestaurantPrinterSetting Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<IEnumerable<RestaurantPrinterSetting>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantPrinterSetting>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "RestaurantBranch ");
        }
        public async Task<IEnumerable<RestaurantPrinterSetting>> GetByRestaurantIdAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == RestaurantId, ChildObjects: "Restaurant ,RestaurantBranch ");
        }
        public async Task<IEnumerable<RestaurantPrinterSetting>> GetByRestaurantBranchIdAsync(long RestaurantBranchId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "RestaurantBranch");
        }

        public async Task<RestaurantPrinterSetting> GetByTypeAndRestaurantBranchIdAsync(long RestaurantBranchId, string Type)
        {
            var list = await _repo.GetByIdAsync(x => x.RestaurantBranchId == RestaurantBranchId && x.Type == Type);

            var printer = list.FirstOrDefault(x => x.IsDefault);
            if (printer == null)
                printer = list.FirstOrDefault();

            return printer;
        }

    }
}
