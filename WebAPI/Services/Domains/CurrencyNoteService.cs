using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CurrencyNoteService: ICurrencyNoteService
    {
        private readonly ICurrencyNoteRepo _repo;
        public CurrencyNoteService(ICurrencyNoteRepo repo)
        {
            _repo=repo;
        }

        public async Task<IEnumerable<CurrencyNote>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<CurrencyNote>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<CurrencyNote> AddCurrencyNote(CurrencyNote Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<CurrencyNote> UpdateCurrencyNote(CurrencyNote Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<CurrencyNote> ArchiveCurrencyNote(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
