using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartPartnersManagementService
    {
        Task<IEnumerable<SparePartPartnersManagement>> GetAllAsync();
        Task<long> GetAllPartnersBySparePartDealerIdAsync(long SparePartDealerId);
        Task<long> GetPositionCount(long GarageId);
        Task<IEnumerable<SparePartPartnersManagement>> GetSparePartPartnersManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartPartnersManagement>> GetSparePartPartnersManagementBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartPartnersManagement> AddSparePartPartnersManagementAsync(SparePartPartnersManagement Model);
        Task<SparePartPartnersManagement> UpdateSparePartPartnersManagementAsync(SparePartPartnersManagement Model);
        Task<SparePartPartnersManagement> ArchiveSparePartPartnersManagementAsync(long Id);
    }
}
