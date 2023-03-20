using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCategoryWiseSaleService
    {
        Task<IEnumerable<RestaurantCategoryWiseSale>> GetAllAsync();
        Task<IEnumerable<RestaurantCategoryWiseSale>> GetByIdAsync(long Id);
        Task<IEnumerable<RestaurantCategoryWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId);
        Task<IEnumerable<RestaurantCategoryWiseSale>> GetOrderDetailIdAsync(long OrderDetailId);
        Task<IEnumerable<RestaurantCategoryWiseSale>> GetCategoryIdAsync(long CategoryId);
        Task<RestaurantCategoryWiseSale> AddCategoryWiseSale(RestaurantCategoryWiseSale Model);
        Task<RestaurantCategoryWiseSale> UpdateCategoryWiseSale(RestaurantCategoryWiseSale Model);
        Task<RestaurantCategoryWiseSale> ArchiveCategoryWiseSale(long Id);
    }
}
