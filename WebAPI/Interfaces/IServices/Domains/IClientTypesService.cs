using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientTypesService
    {
        Task<IEnumerable<ClientTypes>> GetAllCitiesAsync();
        Task<IEnumerable<ClientTypes>> GetCityByIdAsync(long Id);
        Task<ClientTypes> AddCityAsync(ClientTypes Entity);
        Task<ClientTypes> UpdateCityAsync(ClientTypes Entity);
        Task<ClientTypes> ArchiveCityAsync(long Id);
    }
}
