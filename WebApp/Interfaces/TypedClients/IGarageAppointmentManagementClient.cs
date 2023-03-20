using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageAppointmentManagementClient
    {
        Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageAppointmentManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageAppointmentManagementDTO> AddGarageAppointmentManagementAsync(GarageAppointmentManagementDTO Entity);
        Task<GarageAppointmentManagementDTO> UpdateGarageAppointmentManagementAsync(GarageAppointmentManagementDTO Entity);
        Task DeleteGarageAppointmentManagementAsync(long Id);
    }
}
