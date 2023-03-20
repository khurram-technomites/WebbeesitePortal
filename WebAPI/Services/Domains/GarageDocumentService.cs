using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageDocumentService : IGarageDocumentService
    {
        private readonly IGarageDocumentRepo _repo;

        public GarageDocumentService(IGarageDocumentRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageDocument> AddDocument(GarageDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteRecord(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<GarageDocument> EditDocument(GarageDocument Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<GarageDocument>> GetByGarage(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId);
        }

        public async Task<IEnumerable<GarageDocument>> GetByID(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageDocument>> GetByPath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Path == Path);
        }
    }
}
