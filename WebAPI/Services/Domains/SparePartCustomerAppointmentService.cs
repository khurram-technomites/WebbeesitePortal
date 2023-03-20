using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartCustomerAppointmentService : ISparePartCustomerAppointmentService
    {
        private readonly ISparePartCustomerAppointmentRepo _repo;
        public SparePartCustomerAppointmentService(ISparePartCustomerAppointmentRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartCustomerAppointment>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartCustomerAppointment>> GetSparePartCustomerAppointByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartCustomerAppointment>> GetSparePartCustomerAppointBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartCustomerAppointment> AddSparePartCustomerAppointAsync(SparePartCustomerAppointment Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartCustomerAppointment> UpdateSparePartCustomerAppointAsync(SparePartCustomerAppointment Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartCustomerAppointment> ArchiveSparePartCustomerAppointAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
