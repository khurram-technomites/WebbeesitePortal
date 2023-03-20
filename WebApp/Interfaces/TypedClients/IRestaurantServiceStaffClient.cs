using HelperClasses.DTOs;
using HelperClasses.DTOs.RestaurantServiceStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IRestaurantServiceStaffClient
    {
        Task<IEnumerable<RestaurantServiceStaffDTO>> GetAllRestaurantServiceStaffsAsync(PagingParameters paging);
        Task<IEnumerable<RestaurantServiceStaffDTO>> GetAllRestaurantServiceStaffsAsync();
        Task<IEnumerable<RestaurantServiceStaffDTO>> GetRestaurantServiceStaffByRestaurantIdAsync(long ServiceStaffId);
        Task<RestaurantServiceStaffDTO> GetRestaurantServiceStaffByIdAsync(long ServiceStaffId);
        Task<RestaurantServiceStaffDTO> AddRestaurantServiceStaffAsync(RestaurantServiceStaffDTO Entity);
        Task<RestaurantServiceStaffDTO> UpdateRestaurantServiceStaffAsync(RestaurantServiceStaffDTO Entity);
        Task DeleteRestaurantServiceStaffAsync(long ServiceStaffId);
        Task<RestaurantServiceStaffDTO> ToggleActiveStatus(long Id);
    }
}
