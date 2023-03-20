using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageMenuService
    {
        Task<IEnumerable<GarageMenu>> GetAllAsync();
        Task<IEnumerable<GarageMenu>> GetGarageMenuByIdAsync(long Id);
        Task<GarageMenu> AddGarageMenuAsync(GarageMenu Model);
        Task<GarageMenu> UpdateGarageMenuAsync(GarageMenu Model);
        Task<GarageMenu> ArchiveGarageMenuAsync(long Id);
        Task<IEnumerable<GarageMenu>> GetMenuByGarageId(long GarageId);
    }
}
