using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class SparePartFAQService : ISparePartFAQService
    {
        private readonly ISparePartFAQRepo _repo;
        public SparePartFAQService(ISparePartFAQRepo repo)
        {
            _repo = repo;
        }

        public async Task<SparePartFAQ> AddFAQAsync(SparePartFAQ Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartFAQ> ArchiveFAQAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartFAQ>> GetFAQBySparePartAsync(long SparePartId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartId == SparePartId);
        }

        public async Task<IEnumerable<SparePartFAQ>> GetFAQByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<long> MaxCount(long SparePartId)
        {
            return await _repo.GetCount(x => x.SparePartId == SparePartId);
        }

        public async Task<SparePartFAQ> UpdateFAQAsync(SparePartFAQ Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
