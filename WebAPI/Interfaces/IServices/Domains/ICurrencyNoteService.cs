using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICurrencyNoteService
    {
        Task<IEnumerable<CurrencyNote>> GetAllAsync();
        Task<IEnumerable<CurrencyNote>> GetByIdAsync(long Id);
        Task<CurrencyNote> AddCurrencyNote(CurrencyNote Model);
        Task<CurrencyNote> UpdateCurrencyNote(CurrencyNote Model);
        Task<CurrencyNote> ArchiveCurrencyNote(long Id);
    }
}
