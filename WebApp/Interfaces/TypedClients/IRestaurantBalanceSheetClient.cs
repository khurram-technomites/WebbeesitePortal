using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantBalanceSheetClient
    {
        Task<IEnumerable<RestaurantBalanceSheetLogsDTO>> GetRestaurantBalanceSheetLogsByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<RestaurantBalanceSheetLogsDTO>> GetRestaurantBalanceSheetLogsByBranchAsync(long RestaurantBranchId);
        Task<RestaurantBalanceSheetLogsDTO> GetByIdAsync(long Id);
    }
}
