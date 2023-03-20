using HelperClasses.Classes;
using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartRequestQuoteService: ISparePartRequestQuoteService
    {
        private readonly ISparePartRequestQuoteRepo _repo;

        public SparePartRequestQuoteService(ISparePartRequestQuoteRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartRequestQuote>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<long> GetTotalCount()
        {
            return await _repo.GetCount();
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "SparePartsDealer , SparePartRequest , SparePartRequestQuoteImages ,SparePartRequest.SparePartRequestImages , SparePartRequest.CarMake , SparePartRequest.CarModel" , OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetBySparePartsDealerIdAsync(long sparePartsDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == sparePartsDealerId, ChildObjects: "SparePartsDealer,SparePartRequest,SparePartRequestQuoteImages" , OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }

        public async Task<IEnumerable<SparePartRequestQuote>> GetBySparePartRequestIdAsync(long sparePartRequestId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartRequestId == sparePartRequestId, ChildObjects: "SparePartsDealer , SparePartRequest , SparePartRequestQuoteImages" ,OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetPendingQuotesBySparePartRequestIdAsync(long sparePartDealertId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == sparePartDealertId && x.SparePartRequest.Status  == Enum.GetName(typeof(StatusforGarageRequest), StatusforGarageRequest.Pending) , ChildObjects: "SparePartsDealer , SparePartRequest , SparePartRequestQuoteImages" ,
                OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetQuotesBySparePartRequestIdAsync(long sparePartRequestId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == sparePartRequestId, ChildObjects: "SparePartsDealer , SparePartRequest , SparePartRequestQuoteImages" , OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<SparePartRequestQuote> AddSparePartRequestQuoteAsync(SparePartRequestQuote Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartRequestQuote> UpdateSparePartRequestQuoteAsync(SparePartRequestQuote Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<SparePartRequestQuote> ArchiveSparePartRequestQuoteAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartAvailableRequests>> GetAllByUserAndFilterAsync(string UserId, SparePartQuoteFilter Filter)
        {
            return await _repo.GetByUserAndFilter(UserId, Filter);
        }
        public async Task<IEnumerable<SparePartRequestQuote>> GetAllQuoteForFilterAsync(long SpareSpareDealerId, SparePartQuoteFilter Filter)
        {
            return await _repo.GetByQuoteFilter(SpareSpareDealerId, Filter);
        }
    }
}