using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGaragePartnersManagementService
    {
        Task<IEnumerable<GaragePartnersManagement>> GetAllAsync();
        Task<long> GetAllPartnersByGarageIdAsync(long GarageId);
        Task<long> GetPositionCount(long GarageId);
        Task<IEnumerable<GaragePartnersManagement>> GetGaragePartnersManagementByIdAsync(long Id);
        Task<IEnumerable<GaragePartnersManagement>> GetGaragePartnersManagementByGarageIdAsync(long GaragedId);
        Task<long> GetGaragePartnersManagementCountByGarageIdAsync(long GaragedId);
        Task<GaragePartnersManagement> AddGaragePartnersManagementAsync(GaragePartnersManagement Model);
        Task<GaragePartnersManagement> UpdateGaragePartnersManagementAsync(GaragePartnersManagement Model);
        Task<GaragePartnersManagement> ArchiveGaragePartnersManagementAsync(long Id);
    }
}
