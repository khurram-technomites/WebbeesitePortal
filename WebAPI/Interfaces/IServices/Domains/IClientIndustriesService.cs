using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientIndustriesService
    {
        Task<IEnumerable<ClientIndustries>> GetAllClientIndustriesAsync();
        Task<IEnumerable<ClientIndustries>> GetClientIndustriesByIdAsync(long Id);
        Task<ClientIndustries> AddClientIndustriesAsync(ClientIndustries Entity);
        Task<ClientIndustries> UpdateClientIndustriesAsync(ClientIndustries Entity);
        Task<ClientIndustries> ArchiveClientIndustriesAsync(long Id);
    }
}
