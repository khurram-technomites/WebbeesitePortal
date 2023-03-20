using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageCustomerAppointmentService: IGarageCustomerAppointmentService
    {
        private readonly IGarageCustomerAppointmentRepo _repo;
        public GarageCustomerAppointmentService(IGarageCustomerAppointmentRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageCustomerAppointment>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageCustomerAppointment>> GetGarageCustomerAppointmentByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageCustomerAppointment>> GetGarageCustomerAppointmentByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageCustomerAppointment> AddGarageCustomerAppointmentAsync(GarageCustomerAppointment Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageCustomerAppointment> UpdateGarageCustomerAppointmentAsync(GarageCustomerAppointment Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageCustomerAppointment> ArchiveGarageCustomerAppointmentAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
