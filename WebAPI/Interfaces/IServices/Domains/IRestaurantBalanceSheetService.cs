using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantBalanceSheetService
    {
        Task<IEnumerable<RestaurantBalanceSheet>> GetAllAsync();
        Task<RestaurantBalanceSheet> GetByIdAsync(long id);
        //Task<RestaurantBalanceSheet> GetShiftDetails(RestaurantBalanceSheet balanceSheet);
        Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantIdAsync(long RestaurantId);
        Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantCashierStaffIdAsync(long CashierStaffId);
        Task<IEnumerable<RestaurantBalanceSheet>> GetByRestaurantCashierStaffIdAsync(long CashierStaffId, string deviceId);
        Task<RestaurantBalanceSheetReportDTO> GetReportByRestaurantCashierStaffIdAsync(long CashierStaffId, long? Id);
        Task<RestaurantBalanceSheet> AddRestaurantBalanceSheetAsync(RestaurantBalanceSheet Model);
        Task<RestaurantBalanceSheet> UpdateRestaurantBalanceSheetAsync(RestaurantBalanceSheet Model);
        Task<RestaurantBalanceSheet> ArchiveRestaurantBalanceSheetAsync(long Id);
    }
}
