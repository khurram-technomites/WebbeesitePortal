using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantUserLogManagementService
    {
        Task<IEnumerable<RestaurantUserLogManagement>> GetAllAsync();
        Task<RestaurantUserLogManagement> GetByIdAsync(long id);
        Task<IEnumerable<RestaurantUserLogManagement>> GetByRestaurantIdAsync(long RestaurantId);
        //Task<IEnumerable<RestaurantUserLogManagement>> GetByServiceStaffIdAsync(long ServiceStaffId);
        Task<IEnumerable<RestaurantUserLogManagement>> GetByRestaurantBranchIdAsync(long RestaurantBranchId);
        Task<RestaurantUserLogManagement> AddRestaurantUserLogManagementArgumentsAsync(DateTime? LoginTime, string DeviceID, string UserId, string UserType, long? UserDetailId, long RestaurantId, long RestaurantBranchId);
        Task<RestaurantUserLogManagement> AddRestaurantUserLogManagementAsync(RestaurantUserLogManagement Model);
        Task<RestaurantUserLogManagement> UpdateRestaurantUserLogManagementArgumentsAsync(DateTime? LogoutTime, string UserId, string UserType);
        Task<RestaurantUserLogManagement> UpdateRestaurantUserLogManagementAsync(RestaurantUserLogManagement Model);
        Task<RestaurantUserLogManagement> ArchiveRestaurantUserLogManagementAsync(long Id);
    }
}
