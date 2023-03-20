using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepo _repo;
        public CountryService(ICountryRepo repo)
        {
            _repo = repo;
        }

        public async Task<Country> AddCountryAsync(Country Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Country> ArchiveCountryAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Country>> GetAllCountrysAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<Country>> GetCountryByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<Country> UpdateCountryAsync(Country Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}

