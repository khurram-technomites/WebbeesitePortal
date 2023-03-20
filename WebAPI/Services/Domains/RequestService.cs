using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepo _repo;
        private readonly ISparePartRequestQuoteRepo _quoteRepo;
        private readonly ISparePartRequestImagesRepo _imagesRepo;
        public RequestService(IRequestRepo repo , ISparePartRequestQuoteRepo quoteRepo, ISparePartRequestImagesRepo imagesRepo)
        {
            _repo = repo;
            _quoteRepo = quoteRepo;
            _imagesRepo = imagesRepo;
        }

        public async Task<SparePartRequest> AddRequestAsync(SparePartRequest Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<SparePartRequest> ArchiveRequestAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartRequest>> GetAllRequestsAsync(SparePartRequestFilter Filter, string UserId)
        {
            if (Filter.ActiveRequestsOnly)
                return await _repo.GetByIdAsync(x => x.Status != Enum.GetName(typeof(SparePartRequestStatus), SparePartRequestStatus.Delivered) && x.CreatedBy == UserId , Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "SparePartRequestImages", IsOrderByDescending: true);

            else
                return await _repo.GetByIdAsync(x => x.Status == Enum.GetName(typeof(SparePartRequestStatus), SparePartRequestStatus.Delivered) && x.CreatedBy == UserId, Pagination: Filter.Paging, OrderExp: x => x.Id, ChildObjects: "SparePartRequestImages", IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequest>> GetAllRequestForFilterAsync(SparePartRequestFilter Filter, long GarageId)
        {
            return await _repo.GetByUserAndFilter(GarageId, Filter );
        }
        public async Task<IEnumerable<SparePartAvailableRequests>> GetRequestByActiveStatusAsync(string UserId, string Status)
        {

            return await _repo.GetAvailableRequestByStatus(UserId , Status);
        }
        public async Task<IEnumerable<SparePartAvailableRequests>> GetAvailableRequestByCarMake(long CarMakeId)
        {
            return await _repo.GetAvailableRequestByCarMake(CarMakeId);
        }

        public async Task<IEnumerable<SparePartRequest>> GetRequestByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "SparePartRequestImages , SparePartRequestQuotes, Garage , CarMake , CarModel", OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequest>> GetRequestBySparePartRequestQuoteId(long SparePartRequestQuoteId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartRequestQuoteId == SparePartRequestQuoteId, ChildObjects: "SparePartRequestImages , SparePartRequestQuotes , CarMake , CarModel", OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequest>> GetActiveRequestByGarageIdAsync(SparePartRequestFilter Filter ,long GarageId)
        {
                return await _repo.GetByIdAsync(x => x.GarageId == GarageId && x.SparePartRequestQuoteId == null && x.Status == Enum.GetName(typeof(StatusforGarageRequest), StatusforGarageRequest.Pending), Pagination: Filter.Paging, OrderExp: x => x.CreationDate, IsOrderByDescending: true, ChildObjects: "SparePartRequestImages , SparePartRequestQuotes , CarMake , CarModel ");

        }
        public async Task<IEnumerable<SparePartRequest>> GetAllRequestByGarageIdAsync(long GarageId , SparePartRequestFilter Filter)
        {
            var allRequest = await _repo.GetByIdAsync(x => x.GarageId == GarageId, Pagination: Filter.Paging, ChildObjects: "SparePartRequestImages , SparePartRequestQuotes , CarMake , CarModel ", OrderExp: x => x.CreationDate, IsOrderByDescending: true);
            return allRequest;
        }
        public async Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.MulkiyaImageFront == Path || x.MulkiyaImageBack == Path,OrderExp :x => x.CreationDate, IsOrderByDescending: true);
        }

        public async Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaBackAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.MulkiyaImageBack == Path);
        }

        public async Task<IEnumerable<SparePartRequest>> GetRequestByMulkiyaFrontAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.MulkiyaImageFront == Path);
        }

        public async Task<IEnumerable<string>> GetSparePartDealersByCarMake(long CarMakeId)
        {
            return await _repo.GetSparePartDealersByCarMake(CarMakeId);
        }
        public async Task<IEnumerable<string>> GetSparePartDealersBySparePartRequestId(long SparePartRequestId)
        {
            return await _repo.GetSparePartDealersBySparePartRequestId(SparePartRequestId);
        }
        //GetSparePartDealersBySparePartRequestId
        public async Task<SparePartRequest> UpdateRequestAsync(SparePartRequest Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
        public async Task<long> GetTotalCount()
        {
            return await _repo.GetCount();
        }
        public async Task DeleteGarageRequestImage(long Id)
        {
            await _imagesRepo.DeleteAsync(Id);
        }
    }
}
