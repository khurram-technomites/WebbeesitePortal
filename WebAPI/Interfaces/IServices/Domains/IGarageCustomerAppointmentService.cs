using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageCustomerAppointmentService
    {
        Task<IEnumerable<GarageCustomerAppointment>> GetAllAsync();
        Task<IEnumerable<GarageCustomerAppointment>> GetGarageCustomerAppointmentByIdAsync(long Id);
        Task<IEnumerable<GarageCustomerAppointment>> GetGarageCustomerAppointmentByGarageIdAsync(long GaragedId);
        Task<GarageCustomerAppointment> AddGarageCustomerAppointmentAsync(GarageCustomerAppointment Model);
        Task<GarageCustomerAppointment> UpdateGarageCustomerAppointmentAsync(GarageCustomerAppointment Model);
        Task<GarageCustomerAppointment> ArchiveGarageCustomerAppointmentAsync(long Id);
    }
}
