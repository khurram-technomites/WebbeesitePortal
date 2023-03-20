using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICityClient
    {
        Task<IEnumerable<CityDTO>> GetCities();
        Task<CityDTO> GetCityByID(long Id);
        Task<CityDTO> Create(CityDTO model);
        Task<CityDTO> Edit(CityDTO model);
        Task<IEnumerable<CityDTO>> GetCityByCountryId(long Id);
        Task<CityDTO> Delete(long Id);
        Task<CityDTO> ToggleActiveStatus(long CityId);
        Task<IEnumerable<CityDTO>> GetCitiesMaster();

    }
}
