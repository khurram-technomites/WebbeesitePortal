using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartCustomerAppointmentClient
    {
        Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllAsync();
        Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartCustomerAppointmentDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCustomerAppointmentDTO> AddSparePartCustomerAppointmentAsync(SparePartCustomerAppointmentDTO Entity);
        Task<SparePartCustomerAppointmentDTO> UpdateSparePartCustomerAppointmentAsync(SparePartCustomerAppointmentDTO Entity);
        Task DeleteSparePartCustomerAppointmenttAsync(long Id);
    }
}
