using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;


namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IDealerScheduleService 
    {
        Task<IEnumerable<DealerSchedule>> GetAllDealerSchedulesAsync(PagingParameters Pagination);
        Task<IEnumerable<DealerSchedule>> GetDealerScheduleByIdAsync(long Id);
        Task<DealerSchedule> AddDealerScheduleAsync(DealerSchedule Entity);
        Task<DealerSchedule> UpdateDealerScheduleAsync(DealerSchedule Entity);
        Task<DealerSchedule> ArchiveDealerScheduleAsync(long Id);
    }
}
