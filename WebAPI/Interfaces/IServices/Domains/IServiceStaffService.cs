using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IServiceStaffService
    {
        Task<IEnumerable<ServiceStaff>> GetAllServiceStaffAsync(PagingParameters Pagination);
        Task<long> GetAllServiceStaffsCountAsync();
        Task<long> GetAllServiceStaffsCountByRestaurantIdAsync(long restaurantId);
        Task<IEnumerable<ServiceStaff>> GetServiceStaffByIdAsync(long Id);
        Task<IEnumerable<ServiceStaff>> GetServiceStaffByUserAsync(string UserId);
        Task<ServiceStaff> AddServiceStaffAsync(ServiceStaff Entity);
        Task<ServiceStaff> UpdateServiceStaffAsync(ServiceStaff Entity);
        Task<ServiceStaff> ArchiveServiceStaffAsync(long Id);
    }
}
