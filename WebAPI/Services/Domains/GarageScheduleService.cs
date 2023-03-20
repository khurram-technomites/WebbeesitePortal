using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageScheduleService : IGarageScheduleService
    {
        private readonly IGarageScheduleRepo _repo;

        public GarageScheduleService(IGarageScheduleRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageSchedule>> AddAndUpdateRange(IEnumerable<GarageSchedule> List)
        {
            return await _repo.UpdateRangeAsync(List);
        }

        public async Task DeleteSchedule(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<GarageSchedule>> GetByGarage(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId);
        }

        public async Task<IEnumerable<GarageSchedule>> GetByGarageAndDay(long GarageId, string Day)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId && x.Day == Day);
        }
    }
}
