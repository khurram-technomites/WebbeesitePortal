using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientSectionsService
    {
        Task<IEnumerable<ClientSections>> GetAllCitiesAsync();
        Task<IEnumerable<ClientSections>> GetCityByIdAsync(long Id);
        Task<ClientSections> AddCityAsync(ClientSections Entity);
        Task<ClientSections> UpdateCityAsync(ClientSections Entity);
        Task<ClientSections> ArchiveCityAsync(long Id);
    }
}
