using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCashDenominationDetailService
    {
        Task<IEnumerable<RestaurantCashDenominationDetail>> GetAllAsync();
        Task<IEnumerable<RestaurantCashDenominationDetail>> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantCashDenominationDetail>> GetByCurrencyNoteIdAsync(long CurrencyNoteId);
        Task<IEnumerable<RestaurantCashDenominationDetail>> GetByCashDenominationIdAsync(long CashDenominationId);
        Task<RestaurantCashDenominationDetail> AddCashDenominationDetail(RestaurantCashDenominationDetail Model);
        Task<RestaurantCashDenominationDetail> UpdateCashDenominationDetail(RestaurantCashDenominationDetail Model);
        Task<RestaurantCashDenominationDetail> ArchiveCashDenominationDetail(long Id);
    }
}
