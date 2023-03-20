using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantCategoryWiseSaleService: IRestaurantCategoryWiseSaleService
    {
        private readonly IRestaurantCategoryWiseSaleRepo _repo;
        public RestaurantCategoryWiseSaleService(IRestaurantCategoryWiseSaleRepo repo)
        {
            _repo=repo;
        }
        public async Task<IEnumerable<RestaurantCategoryWiseSale>> GetAllAsync()
        {
            return await _repo.GetAllAsync();   
        }
        public async Task<IEnumerable<RestaurantCategoryWiseSale>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x=>x.Id==Id);
        }
        public async Task<IEnumerable<RestaurantCategoryWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantBalanceSheetId == BalanceSheetId,ChildObjects: "RestaurantBalanceSheet");
        }
        public async Task<IEnumerable<RestaurantCategoryWiseSale>> GetOrderDetailIdAsync(long OrderDetailId)
        {
            return await _repo.GetByIdAsync(x => x.OrderDetailId == OrderDetailId, ChildObjects: "OrderDetail");
        }
        public async Task<IEnumerable<RestaurantCategoryWiseSale>> GetCategoryIdAsync(long CategoryId)
        {
            return await _repo.GetByIdAsync(x => x.CategoryId == CategoryId, ChildObjects: "Category");
        }
        public async Task<RestaurantCategoryWiseSale> AddCategoryWiseSale(RestaurantCategoryWiseSale Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<RestaurantCategoryWiseSale> UpdateCategoryWiseSale(RestaurantCategoryWiseSale Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<RestaurantCategoryWiseSale> ArchiveCategoryWiseSale(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
