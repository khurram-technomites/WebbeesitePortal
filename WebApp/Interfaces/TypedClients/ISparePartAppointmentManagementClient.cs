using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartAppointmentManagementClient
    {
        Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartAppointmentManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartAppointmentManagementDTO> AddSparePartAppointmentManagementAsync(SparePartAppointmentManagementDTO Entity);
        Task<SparePartAppointmentManagementDTO> UpdateSparePartAppointmentManagementAsync(SparePartAppointmentManagementDTO Entity);
        Task DeleteSparePartAppointmentManagementAsync(long Id);
    }
}
