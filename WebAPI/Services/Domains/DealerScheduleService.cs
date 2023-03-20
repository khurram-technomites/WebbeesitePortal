
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class DealerScheduleService : IDealerScheduleService
    {
        private readonly IDealerScheduleRepo _repo;
        public DealerScheduleService(IDealerScheduleRepo repo)
        {
            _repo = repo;
        }

        public async Task<DealerSchedule> AddDealerScheduleAsync(DealerSchedule Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<DealerSchedule> ArchiveDealerScheduleAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<DealerSchedule>> GetAllDealerSchedulesAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination, OrderExp: x => x.Id, ChildObjects: "User");
        }

        public async Task<IEnumerable<DealerSchedule>> GetDealerScheduleByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<DealerSchedule> UpdateDealerScheduleAsync(DealerSchedule Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
