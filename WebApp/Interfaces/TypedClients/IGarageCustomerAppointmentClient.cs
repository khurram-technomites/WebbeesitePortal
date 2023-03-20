using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageCustomerAppointmentClient
    {
        Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllAsync();
        Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageCustomerAppointmentDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageCustomerAppointmentDTO> AddGarageCustomerAppointmentAsync(GarageCustomerAppointmentDTO Entity);
        Task<GarageCustomerAppointmentDTO> UpdateGarageCustomerAppointmentAsync(GarageCustomerAppointmentDTO Entity);
        Task DeleteGarageCustomerAppointmentAsync(long Id);
    }
}
