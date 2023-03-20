using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartDealerScheduleService
    {
        Task<IEnumerable<DealerSchedule>> GetByDealerAndDay(long DealerId, string Day);
        Task DeleteSchedule(long Id);

        Task<IEnumerable<DealerSchedule>> GetSparePartDealerScheduleByDealer(long branchId);
        Task<DealerSchedule> AddSparePartDealerScheduleAsync(DealerSchedule Model);
        Task<IEnumerable<DealerSchedule>> GetSparePartDealerScheduleById(long id);
        Task<DealerSchedule> UpdateSparePartDealerBranchScheduleAsync(DealerSchedule Model);
        Task ArchiveSparePartDealerScheduleAsync(long Id);
        Task<IEnumerable<DealerSchedule>> GetScheduleByDay(string day, TimeSpan openingTime, TimeSpan closingTime, long branchId, long Id = 0);
    }
}
