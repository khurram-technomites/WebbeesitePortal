using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class BusinessSettingService : IBusinessSettingService
    {
        private readonly IBusinessSettingRepo _repo;
        public BusinessSettingService(IBusinessSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<BusinessSettings> AddBusinessSettingAsync(BusinessSettings Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<BusinessSettings> ArchiveBusinessSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
      
        public async Task<IEnumerable<BusinessSettings>> GetAllBusinessSettingAsync()
        {
            return await _repo.GetAllAsync(Pagination: null, OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<BusinessSettings>> GetBusinessSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<BusinessSettings> UpdateBusinessSettingAsync(BusinessSettings Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
