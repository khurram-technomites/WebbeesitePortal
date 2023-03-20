using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantCashDenominationService: IRestaurantCashDenominationService
    {
        private readonly IRestaurantCashDenominationRepo _repo;
        public RestaurantCashDenominationService(IRestaurantCashDenominationRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RestaurantCashDenomination>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<RestaurantCashDenomination>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<RestaurantCashDenomination>> GetByBalanceSheetId(long BalanceSheetId)
        {
            return await _repo.GetByIdAsync(x=>x.RestaurantBalanceSheetId == BalanceSheetId,ChildObjects: "RestaurantBalanceSheet");
        }

        public async Task<RestaurantCashDenomination> AddCashDenominationAsync(RestaurantCashDenomination Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<RestaurantCashDenomination> UpdateCashDenominationAsync(RestaurantCashDenomination Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<RestaurantCashDenomination> ArchiveCashDenominationAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

    }
}
