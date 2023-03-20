using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCashDenominationService
    {
        Task<IEnumerable<RestaurantCashDenomination>> GetAllAsync();
        Task<IEnumerable<RestaurantCashDenomination>> GetByIdAsync(long Id);
        Task<IEnumerable<RestaurantCashDenomination>> GetByBalanceSheetId(long BalanceSheetId);
        Task<RestaurantCashDenomination> AddCashDenominationAsync(RestaurantCashDenomination Model);
        Task<RestaurantCashDenomination> UpdateCashDenominationAsync(RestaurantCashDenomination Model);
        Task<RestaurantCashDenomination> ArchiveCashDenominationAsync(long  Id);
    }
}
