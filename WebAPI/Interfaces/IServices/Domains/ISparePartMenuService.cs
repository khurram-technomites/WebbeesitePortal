using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartMenuService
    {
        Task<IEnumerable<SparePartMenu>> GetAllAsync();
        Task<IEnumerable<SparePartMenu>> GetSparePartMenuById(long Id);
        Task<IEnumerable<SparePartMenu>> GetSparePartMenuBySparepartDealerId(long SparepartDealerId);
        Task<SparePartMenu> AddSparePartMenuAsync(SparePartMenu Model);
        Task<SparePartMenu> UpdateSparePartMenuAsync(SparePartMenu Model);
        Task<SparePartMenu> ArchiveSparePartMenuAsync(long Id);
    }
}
