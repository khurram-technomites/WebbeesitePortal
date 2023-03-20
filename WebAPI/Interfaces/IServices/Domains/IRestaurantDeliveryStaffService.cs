using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantDeliveryStaffService
    {
        Task<IEnumerable<RestaurantDeliveryStaff>> GetAllRestaurantDeliveryStaffsAsync(long restaurantId);
        Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByIdAsync(long Id);
        Task<RestaurantDeliveryStaff> GetRestaurantDeliveryStaffByUserAsync(string UserId);
        Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByBranchAsync(long BranchId);
        Task<RestaurantDeliveryStaff> AddRestaurantDeliveryStaffAsync(RestaurantDeliveryStaff Entity);
        Task<RestaurantDeliveryStaff> UpdateRestaurantDeliveryStaffAsync(RestaurantDeliveryStaff Entity);
        Task<RestaurantDeliveryStaff> ArchiveRestaurantDeliveryStaffAsync(long Id);
        Task<IEnumerable<RestaurantDeliveryStaff>> GetRestaurantDeliveryStaffByImageAsync(string Path);
    }
}
