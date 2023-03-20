using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICountryClient
    {
        Task<IEnumerable<CountryDTO>> GetCountries();
        Task<CountryDTO> GetCountryByID(long Id);
        Task<IEnumerable<CountryDTO>> GetCountriesByMaster();
        Task<CountryDTO> Create(CountryDTO model);
        Task<CountryDTO> Edit(CountryDTO model);

        Task<CountryDTO> Delete(long Id);

        Task<CountryDTO> ToggleActiveStatus(long CountryId);


    }
}
