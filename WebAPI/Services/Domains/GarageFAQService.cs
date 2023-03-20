using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageFAQService : IGarageFAQService
    {
        private readonly IGarageFAQRepo _repo;

        public GarageFAQService(IGarageFAQRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageFAQ> AddFAQAsync(GarageFAQ Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageFAQ> ArchiveFAQAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageFAQ>> GetFAQByGarageAsync(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId && x.ArchivedDate == null);
        }

        public async Task<IEnumerable<GarageFAQ>> GetFAQByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<long> MaxCount(long GarageId)
        {
            return await _repo.GetCount(x => x.GarageId == GarageId);
        }

        public async Task<GarageFAQ> UpdateFAQAsync(GarageFAQ Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
