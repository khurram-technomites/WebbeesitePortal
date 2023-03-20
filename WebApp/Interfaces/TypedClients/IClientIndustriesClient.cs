using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IClientIndustriesClient
    {
        Task<IEnumerable<ClientIndustriesDTO>> GetIndustries();
        Task<ClientIndustriesDTO> GetCityByID(long Id);
        Task<ClientIndustriesDTO> Create(ClientIndustriesDTO model);
        Task<ClientIndustriesDTO> Edit(ClientIndustriesDTO model);
        Task<ClientIndustriesDTO> Delete(long Id);
        Task<ClientIndustriesDTO> ToggleActiveStatus(long CityId);
        Task<IEnumerable<ClientIndustriesDTO>> GetCitiesMaster();

    }
}
