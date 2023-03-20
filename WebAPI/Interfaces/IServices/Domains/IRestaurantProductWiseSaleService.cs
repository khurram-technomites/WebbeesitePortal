using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantProductWiseSaleService
    {
        Task<IEnumerable<RestaurantProductWiseSale>> GetAllAsync();
        Task<IEnumerable<RestaurantProductWiseSale>> GetByIdAsync(long Id);
        Task<IEnumerable<RestaurantProductWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId);
        Task<IEnumerable<RestaurantProductWiseSale>> GetOrderDetailIdAsync(long OrderDetailId);
        Task<IEnumerable<RestaurantProductWiseSale>> GetMenuItemIdAsync(long MenuItemId);
        Task<RestaurantProductWiseSale> AddProductWiseSale(RestaurantProductWiseSale Model);
        Task<RestaurantProductWiseSale> UpdateProductWiseSale(RestaurantProductWiseSale Model);
        Task<RestaurantProductWiseSale> ArchiveProductWiseSale(long Id);
    }
}
