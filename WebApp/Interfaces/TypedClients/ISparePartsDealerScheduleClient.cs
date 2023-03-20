using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartsDealerScheduleClient
    {
        Task<IEnumerable<SparePartsDealerScheduleDTO>> GetAllSparePartsDealerSchedulesAsync(long sparePartsDealerId);
        Task<SparePartsDealerScheduleDTO> GetSparePartsDealerScheduleByIdAsync(long id);
        Task<SparePartsDealerScheduleDTO> AddSparePartsDealerScheduleAsync(SparePartsDealerScheduleDTO Entity);
        Task<SparePartsDealerScheduleDTO> UpdateSparePartsDealerScheduleAsync(SparePartsDealerScheduleDTO Entity);
        Task DeleteSparePartsDealerScheduleAsync(long sparePartsDealerId);
    }
}
