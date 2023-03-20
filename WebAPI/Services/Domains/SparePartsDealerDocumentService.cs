using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartsDealerDocumentService : ISparePartsDealerDocumentService
    {
        private readonly ISparePartsDealerDocumentRepo _repo;

        public SparePartsDealerDocumentService(ISparePartsDealerDocumentRepo repo)
        {
            _repo = repo;
        }

        public async Task DeleteRecord(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SparePartsDealerDocument>> GetByPath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Path == Path);
        }
        public async Task<SparePartsDealerDocument> AddSparePartsDocumentAsync(SparePartsDealerDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteSparePartsDocumentAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SparePartsDealerDocument>> GetAllBySparePartsAsync(long SparePartsDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == SparePartsDealerId);
        }

        public async Task<IEnumerable<SparePartsDealerDocument>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<SparePartsDealerDocument> UpdateSparePartsDocumentAsync(SparePartsDealerDocument Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
