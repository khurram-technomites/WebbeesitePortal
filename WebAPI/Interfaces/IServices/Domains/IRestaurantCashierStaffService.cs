
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCashierStaffService
    {
        Task<IEnumerable<RestaurantCashierStaff>> GetAllRestaurantCashierStaffAsync(PagingParameters Pagination);
        Task<IEnumerable<RestaurantCashierStaff>> GetAllRestaurantCashierStaffsAsync(long restaurantId);
        Task<long> GetAllRestaurantCashierStaffsCountAsync(long restaurantId);
        Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByIdAsync(long Id);
        Task<RestaurantCashierStaff> GetRestaurantCashierStaffByUserAsync(string UserId, string deviceId = "");
        Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByBranchAsync(long BranchId);
        Task<RestaurantCashierStaff> AddRestaurantCashierStaffAsync(RestaurantCashierStaff Entity);
        Task<RestaurantCashierStaff> UpdateRestaurantCashierStaffAsync(RestaurantCashierStaff Entity);
        Task<RestaurantCashierStaff> ArchiveRestaurantCashierStaffAsync(long Id);
        Task<IEnumerable<RestaurantCashierStaff>> GetRestaurantCashierStaffByImageAsync(string Path);
    }
}
