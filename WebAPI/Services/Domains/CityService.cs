using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CityService : ICityService
    {
        private readonly ICityRepo _repo;
        public CityService(ICityRepo repo)
        {
            _repo = repo;
        }

        public async Task<City> AddCityAsync(City Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<City> ArchiveCityAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "Country");
        }

        public async Task<IEnumerable<City>> GetAllCitiesByCountry(long CountryId)
        {
            return await _repo.GetAllAsync(x=> x.CountryId == CountryId);
        }


        public async Task<IEnumerable<City>> GetCityByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Country");
        }

        public async Task<IEnumerable<City>> GetCityByLogoAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Logo == Path);
        }

        public async Task<City> UpdateCityAsync(City Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
