using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantServiceStaffService
    {
        Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffAsync(PagingParameters Pagination);
        Task<long> GetAllRestaurantServiceStaffsCountAsync();
        Task<IEnumerable<RestaurantServiceStaff>> GetRestaurantServiceStaffByIdAsync(long Id);
        Task<IEnumerable<RestaurantServiceStaff>> GetRestaurantServiceStaffByUserAsync(string UserId);
        Task<RestaurantServiceStaff> AddRestaurantServiceStaffAsync(RestaurantServiceStaff Entity);
        Task<RestaurantServiceStaff> UpdateRestaurantServiceStaffAsync(RestaurantServiceStaff Entity);
        Task<RestaurantServiceStaff> ArchiveRestaurantServiceStaffAsync(long Id);
        Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffByBranchIdAsync(long RestaurantBranchId);
        Task<IEnumerable<RestaurantServiceStaff>> GetAllRestaurantServiceStaffByRestaurantIdAsync(long RestaurantId);
        Task<RestaurantServiceStaff> GetAllRestaurantServiceStaffByIdAsync(long Id);
    }
}
