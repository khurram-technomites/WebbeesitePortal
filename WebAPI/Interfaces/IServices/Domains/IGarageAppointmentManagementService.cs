using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageAppointmentManagementService
    {
        Task<IEnumerable<GarageAppointmentManagement>> GetAllAsync();
        Task<IEnumerable<GarageAppointmentManagement>> GetGarageGarageAppointmentManagementByIdAsync(long Id);
        Task<IEnumerable<GarageAppointmentManagement>> GetGarageAppointmentManagementByGarageIdAsync(long GaragedId);
        Task<GarageAppointmentManagement> AddGarageAppointmentManagementAsync(GarageAppointmentManagement Model);
        Task<GarageAppointmentManagement> UpdateGarageAppointmentManagementAsync(GarageAppointmentManagement Model);
        Task<GarageAppointmentManagement> ArchiveGarageAppointmentManagementAsync(long Id);
    }
}
