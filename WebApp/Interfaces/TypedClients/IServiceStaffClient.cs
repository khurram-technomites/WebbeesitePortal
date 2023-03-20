using HelperClasses.DTOs;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using HelperClasses.DTOs.ServiceStaff;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IServiceStaffClient
    {
        Task<IEnumerable<ServiceStaffDTO>> GetAllServiceStaffsAsync(PagingParameters paging);
        Task<IEnumerable<ServiceStaffDTO>> GetAllServiceStaffsAsync();
        Task<ServiceStaffDTO> GetServiceStaffByIdAsync(long ServiceStaffId);
        Task<ServiceStaffRegisterDTO> AddServiceStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity);
        Task<ServiceStaffRegisterDTO> UpdateServiceStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity);
        Task DeleteServiceStaffAsync(long ServiceStaffId);
        Task<ServiceStaffDTO> ToggleActiveStatus(long Id);
    }
}
