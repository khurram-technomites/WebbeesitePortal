using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IClientTypesClient
    {
        Task<IEnumerable<ClientTypesDTO>> GetCities();
        Task<ClientTypesDTO> GetCityByID(long Id);
        Task<ClientTypesDTO> Create(ClientTypesDTO model);
        Task<ClientTypesDTO> Edit(ClientTypesDTO model);
        Task<ClientTypesDTO> Delete(long Id);
        Task<ClientTypesDTO> ToggleActiveStatus(long CityId);
        Task<IEnumerable<ClientTypesDTO>> GetCitiesMaster();

    }
}
