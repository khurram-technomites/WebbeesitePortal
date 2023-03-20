using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartCustomerAppointmentService
    {
        Task<IEnumerable<SparePartCustomerAppointment>> GetAllAsync();
        Task<IEnumerable<SparePartCustomerAppointment>> GetSparePartCustomerAppointByIdAsync(long Id);
        Task<IEnumerable<SparePartCustomerAppointment>> GetSparePartCustomerAppointBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCustomerAppointment> AddSparePartCustomerAppointAsync(SparePartCustomerAppointment Model);
        Task<SparePartCustomerAppointment> UpdateSparePartCustomerAppointAsync(SparePartCustomerAppointment Model);
        Task<SparePartCustomerAppointment> ArchiveSparePartCustomerAppointAsync(long Id);
    }
}
