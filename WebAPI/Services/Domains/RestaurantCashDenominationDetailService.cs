using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantCashDenominationDetailService: IRestaurantCashDenominationDetailService
    {
        public readonly IRestaurantCashDenominationDetailRepo _repo;
        public RestaurantCashDenominationDetailService(IRestaurantCashDenominationDetailRepo repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<RestaurantCashDenominationDetail>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }
        public Task<IEnumerable<RestaurantCashDenominationDetail>> GetByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x=>x.Id==Id);
        }

        public Task<IEnumerable<RestaurantCashDenominationDetail>> GetByCurrencyNoteIdAsync(long CurrencyNoteId)
        {
            return _repo.GetByIdAsync(x => x.CurrencyNoteId == CurrencyNoteId,ChildObjects: "CurrencyNote");
        }

        public Task<IEnumerable<RestaurantCashDenominationDetail>> GetByCashDenominationIdAsync(long CashDenominationId)
        {
            return _repo.GetByIdAsync(x=>x.RestaurantCashDenominationId==CashDenominationId,ChildObjects: "RestaurantCashDenomination");
        }

        public Task<RestaurantCashDenominationDetail> AddCashDenominationDetail(RestaurantCashDenominationDetail Model)
        {
            return _repo.InsertAsync(Model);
        }
        public Task<RestaurantCashDenominationDetail> UpdateCashDenominationDetail(RestaurantCashDenominationDetail Model)
        {
            return _repo.UpdateAsync(Model);
        }

        public Task<RestaurantCashDenominationDetail> ArchiveCashDenominationDetail(long Id)
        {
            return _repo.ArchiveAsync(Id);
        }

    }
}
