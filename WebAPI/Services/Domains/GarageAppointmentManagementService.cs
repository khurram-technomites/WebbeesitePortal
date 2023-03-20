using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class GarageAppointmentManagementService: IGarageAppointmentManagementService
    {
        private readonly IGarageAppointmentManagementRepo _repo;

        public GarageAppointmentManagementService(IGarageAppointmentManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageAppointmentManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageAppointmentManagement>> GetGarageGarageAppointmentManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageAppointmentManagement>> GetGarageAppointmentManagementByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageAppointmentManagement> AddGarageAppointmentManagementAsync(GarageAppointmentManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageAppointmentManagement> UpdateGarageAppointmentManagementAsync(GarageAppointmentManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageAppointmentManagement> ArchiveGarageAppointmentManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
