using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartDealerScheduleService : ISparePartDealerScheduleService
    {
        private readonly ISparePartDealerScheduleRepo _repo;

        public SparePartDealerScheduleService(ISparePartDealerScheduleRepo repo)
        {
            _repo = repo;
        }
        public async Task DeleteSchedule(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<DealerSchedule>> GetByDealerAndDay(long DealerId, string Day)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == DealerId && x.Day == Day);
        }
        public async Task<DealerSchedule> AddSparePartDealerScheduleAsync(DealerSchedule Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task ArchiveSparePartDealerScheduleAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<DealerSchedule>> GetSparePartDealerScheduleByDealer(long SparePartsDealerId)
        {
            var result = await _repo.GetAllAsync(x => x.SparePartsDealerId == SparePartsDealerId);
            return result;
        }

        public async Task<IEnumerable<DealerSchedule>> GetSparePartDealerScheduleById(long id)
        {
            return await _repo.GetByIdAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<DealerSchedule>> GetScheduleByDay(string day, TimeSpan openingTime, TimeSpan closingTime, long SparePartsDealerId, long Id = 0)
        {
            return await _repo.GetAllAsync(x => x.Day == day && x.OpeningTime == openingTime && x.ClosingTime == closingTime && x.SparePartsDealerId == SparePartsDealerId && x.Id != Id);
        }

        public async Task<DealerSchedule> UpdateSparePartDealerBranchScheduleAsync(DealerSchedule Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
