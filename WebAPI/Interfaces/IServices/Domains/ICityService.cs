using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<IEnumerable<City>> GetCityByIdAsync(long Id);
        Task<IEnumerable<City>> GetAllCitiesByCountry(long CountryId);
        Task<IEnumerable<City>> GetCityByLogoAsync(string Path);
        Task<City> AddCityAsync(City Entity);
        Task<City> UpdateCityAsync(City Entity);
        Task<City> ArchiveCityAsync(long Id);
    }
}
