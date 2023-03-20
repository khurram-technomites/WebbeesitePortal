using HelperClasses.DTOs;
using HelperClasses.DTOs.DeliveryStaff;
using HelperClasses.DTOs.ServiceAndDeliveryStaffDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IDeliveryStaffClient
    {
        Task<IEnumerable<DeliveryStaffDTO>> GetAllDeliveryStaffsAsync(PagingParameters paging);
        Task<IEnumerable<DeliveryStaffDTO>> GetAllDeliveryStaffsAsync();
        Task<DeliveryStaffDTO> GetDeliveryStaffByIdAsync(long DeliveryStaffId);
        Task<DeliveryStaffRegisterDTO> AddDeliveryStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity);
        Task<DeliveryStaffRegisterDTO> UpdateDeliveryStaffAsync(ServiceAndDeliveryStaffRegisterDTO Entity);
        Task<DeliveryStaffDTO> DeleteDeliveryStaffAsync(long DeliveryStaffId);
        Task<DeliveryStaffDTO> ToggleActiveStatus(long Id);
    }
}
