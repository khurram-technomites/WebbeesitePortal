using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;


namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountrysAsync();
        Task<IEnumerable<Country>> GetCountryByIdAsync(long Id);
        Task<Country> AddCountryAsync(Country Entity);
        Task<Country> UpdateCountryAsync(Country Entity);
        Task<Country> ArchiveCountryAsync(long Id);
    }
}
