using HelperClasses.DTOs.Garage.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IRequestRepo : IRepository<SparePartRequest>
    {
        Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByCarMake(long CarMakeId);
        Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByStatus(string UserId, string Status);
        Task<IEnumerable<string>> GetSparePartDealersByCarMake(long CarMakeId);
        Task<IEnumerable<string>> GetSparePartDealersBySparePartRequestId(long SparePartRequestId);
        Task<IEnumerable<SparePartRequest>> GetByUserAndFilter(long GarageId, SparePartRequestFilter Filter);
    }
}
