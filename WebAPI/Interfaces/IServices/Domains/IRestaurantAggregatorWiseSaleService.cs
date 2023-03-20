using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantAggregatorWiseSaleService
    {
        Task<IEnumerable<RestaurantAggregatorWiseSale>> GetAllAsync();
        Task<IEnumerable<RestaurantAggregatorWiseSale>> GetByIdAsync(long Id);
        Task<IEnumerable<RestaurantAggregatorWiseSale>> GetOrderIdAsync(long OrderId);
        Task<IEnumerable<RestaurantAggregatorWiseSale>> GetBalanceSheetIdAsync(long BalanceSheetId);
        Task<IEnumerable<RestaurantAggregatorWiseSale>> GetRestaurantAggregatorIdAsync(long AggregatorId);
        Task<RestaurantAggregatorWiseSale> AddAggregatorWiseSale(RestaurantAggregatorWiseSale Model);
        Task<RestaurantAggregatorWiseSale> UpdateAggregatorWiseSale(RestaurantAggregatorWiseSale Model);
        Task<RestaurantAggregatorWiseSale> ArchiveAggregatorWiseSale(long Id);
    }
}
