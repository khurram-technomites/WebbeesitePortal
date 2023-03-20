using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartAppointmentManagementService : ISparePartAppointmentManagementService
    {
        private readonly ISparePartAppointmentManagementRepo _repo;
        public SparePartAppointmentManagementService(ISparePartAppointmentManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<SparePartAppointmentManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartAppointmentManagement>> GetSparePartAppointmentManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartAppointmentManagement>> GetSparePartAppointmentManagementBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartAppointmentManagement> AddSparePartAppointmentManagementAsync(SparePartAppointmentManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartAppointmentManagement> UpdateSparePartAppointmentManagementAsync(SparePartAppointmentManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartAppointmentManagement> ArchiveSparePartAppointmentManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }


    }
}
