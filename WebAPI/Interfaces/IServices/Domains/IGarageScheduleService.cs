
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageScheduleService
    {
        Task<IEnumerable<GarageSchedule>> GetByGarageAndDay(long GarageId, string Day);
        Task<IEnumerable<GarageSchedule>> GetByGarage(long GarageId);
        Task DeleteSchedule(long Id);
        Task<IEnumerable<GarageSchedule>> AddAndUpdateRange(IEnumerable<GarageSchedule> List);
    }
}
