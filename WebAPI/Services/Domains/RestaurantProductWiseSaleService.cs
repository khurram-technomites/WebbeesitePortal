using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class RestaurantProductWiseSaleService: IRestaurantProductWiseSaleService
    {
        private readonly IRestaurantProductWiseSaleRepo _repo;
        public RestaurantProductWiseSaleService(IRestaurantProductWiseSaleRepo repo)
        {
            _repo=repo;
        }

        public async Task<IEnumerable<RestaurantProductWiseSale>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<RestaurantProductWiseSale>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x=>x.Id==Id);
        }
        public async Task<IEnumerable<RestaurantProductWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBalanceSheetId == BalanceSheetId,ChildObjects: "RestaurantBalanceSheet");
        }
        public async Task<IEnumerable<RestaurantProductWiseSale>> GetOrderDetailIdAsync(long OrderDetailId)
        {
            return await _repo.GetByIdAsync(x => x.OrderDetailId == OrderDetailId, ChildObjects: "OrderDetail");
        }
        public async Task<IEnumerable<RestaurantProductWiseSale>> GetMenuItemIdAsync(long MenuItemId)
        {
            return await _repo.GetByIdAsync(x => x.MenuItemId == MenuItemId, ChildObjects: "MenuItem");
        }
        public async Task<RestaurantProductWiseSale> AddProductWiseSale(RestaurantProductWiseSale Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<RestaurantProductWiseSale> UpdateProductWiseSale(RestaurantProductWiseSale Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<RestaurantProductWiseSale> ArchiveProductWiseSale(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
