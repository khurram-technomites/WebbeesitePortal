using HelperClasses.DTOs.Restaurant;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IRestaurantBalanceSheetRepo:IRepository<RestaurantBalanceSheet>
    {
        Task<RestaurantBalanceSheetReportDTO> GetReportyRestaurantCashierStaffIdAsync(long CashierStaffId, long? Id);
        //Task<RestaurantBalanceSheet> GetShiftDetails(RestaurantBalanceSheet balanceSheet);
    }
}
