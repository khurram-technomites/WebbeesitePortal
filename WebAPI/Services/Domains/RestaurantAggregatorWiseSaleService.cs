using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantAggregatorWiseSaleService: IRestaurantAggregatorWiseSaleService
    {
        private readonly IRestaurantAggregatorWiseSaleRepo _repo;

        public RestaurantAggregatorWiseSaleService(IRestaurantAggregatorWiseSaleRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RestaurantAggregatorWiseSale>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<RestaurantAggregatorWiseSale>> GetByIdAsync(long Id)
        {
            return await _repo.GetAllAsync(x=>x.Id==Id);
        }
        public async Task<IEnumerable<RestaurantAggregatorWiseSale>> GetOrderIdAsync(long OrderId)
        {
            return await _repo.GetAllAsync(x => x.OrderId == OrderId,ChildObjects: "Order");
        }
        public async Task<IEnumerable<RestaurantAggregatorWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantBalanceSheetId == BalanceSheetId, ChildObjects: "RestaurantBalanceSheet");
        }
        public async Task<IEnumerable<RestaurantAggregatorWiseSale>> GetRestaurantAggregatorIdAsync(long AggregatorId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantAggregatorId == AggregatorId, ChildObjects: "RestaurantAggregator");
        }
        public async Task<RestaurantAggregatorWiseSale> AddAggregatorWiseSale(RestaurantAggregatorWiseSale Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<RestaurantAggregatorWiseSale> UpdateAggregatorWiseSale(RestaurantAggregatorWiseSale Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<RestaurantAggregatorWiseSale> ArchiveAggregatorWiseSale(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
