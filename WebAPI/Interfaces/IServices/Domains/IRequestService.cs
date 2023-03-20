using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRequestService
    {
        Task<IEnumerable<SparePartRequest>> GetAllRequestsAsync(SparePartRequestFilter Filter, string UserId);
        Task<IEnumerable<SparePartRequest>> GetAllRequestForFilterAsync(SparePartRequestFilter Filter, long GarageID);
        Task<IEnumerable<SparePartRequest>> GetRequestByIdAsync(long Id);

        Task<IEnumerable<SparePartRequest>> GetRequestBySparePartRequestQuoteId(long Id);
        Task<IEnumerable<SparePartRequest>> GetActiveRequestByGarageIdAsync(SparePartRequestFilter Filter ,long GarageId );
        Task<IEnumerable<SparePartRequest>> GetAllRequestByGarageIdAsync(long GarageId, SparePartRequestFilter Filter);
        Task<long> GetTotalCount();
        Task<IEnumerable<SparePartAvailableRequests>> GetRequestByActiveStatusAsync(string UserId , string Status);
        Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaAsync(string Path);
        Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaFrontAsync(string Path);
        Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaBackAsync(string Path);
        Task<SparePartRequest> AddRequestAsync(SparePartRequest Entity);
        Task<SparePartRequest> UpdateRequestAsync(SparePartRequest Entity);
        Task<SparePartRequest> ArchiveRequestAsync(long Id);
        Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByCarMake(long CarMakeId);
        Task<IEnumerable<string>> GetSparePartDealersByCarMake(long CarMakeId);
        Task<IEnumerable<string>> GetSparePartDealersBySparePartRequestId(long SparePartRequestId);
        Task DeleteGarageRequestImage(long Id);
    }
}
