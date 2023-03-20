using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartAppointmentManagementService
    {
        Task<IEnumerable<SparePartAppointmentManagement>> GetAllAsync();
        Task<IEnumerable<SparePartAppointmentManagement>> GetSparePartAppointmentManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartAppointmentManagement>> GetSparePartAppointmentManagementBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartAppointmentManagement> AddSparePartAppointmentManagementAsync(SparePartAppointmentManagement Model);
        Task<SparePartAppointmentManagement> UpdateSparePartAppointmentManagementAsync(SparePartAppointmentManagement Model);
        Task<SparePartAppointmentManagement> ArchiveSparePartAppointmentManagementAsync(long Id);
    }
}
