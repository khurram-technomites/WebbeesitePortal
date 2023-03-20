using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IDeliveryStaffService 
    {
        Task<IEnumerable<DeliveryStaff>> GetAllDeliveryStaffsAsync(PagingParameters Pagination);
        Task<long> GetAllDeliveryStaffsCountAsync();
        Task<long> GetAllRestaurantDeliveryStaffsCountAsync(long restaurant);
        Task<IEnumerable<DeliveryStaff>> GetDeliveryStaffByIdAsync(long Id);
        Task<IEnumerable<DeliveryStaff>> GetDeliveryStaffByUserAsync(string UserId);
        Task<DeliveryStaff> AddDeliveryStaffAsync(DeliveryStaff Entity);
        Task<DeliveryStaff> UpdateDeliveryStaffAsync(DeliveryStaff Entity);
        Task<DeliveryStaff> ArchiveDeliveryStaffAsync(long Id);
    }
}
