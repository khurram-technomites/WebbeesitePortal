using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IBusinessSettingService
    {
        Task<IEnumerable<BusinessSettings>> GetAllBusinessSettingAsync();
        Task<IEnumerable<BusinessSettings>> GetBusinessSettingByIdAsync(long Id);
        Task<BusinessSettings> AddBusinessSettingAsync(BusinessSettings Entity);
        Task<BusinessSettings> UpdateBusinessSettingAsync(BusinessSettings Entity);
        Task<BusinessSettings> ArchiveBusinessSettingAsync(long Id);
    }
}
