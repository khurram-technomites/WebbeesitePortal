using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Supplier,Vendor")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryController(ICountryService countryService, IMapper mapper)
        {
            _mapper = mapper;
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<CountryDTO>>(await _countryService.GetAllCountrysAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMasterAsync()
        {
            return Ok(_mapper.Map<IEnumerable<CountryDTO>>(await _countryService.GetAllCountrysAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<CountryDTO> countries = _mapper.Map<IEnumerable<CountryDTO>>(await _countryService.GetCountryByIdAsync(Id));
            return Ok(countries.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CountryDTO Model)
        {
            return Ok(_mapper.Map<CountryDTO>(await _countryService.AddCountryAsync(_mapper.Map<Country>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CountryDTO Model)
        {
            return Ok(_mapper.Map<CountryDTO>(await _countryService.UpdateCountryAsync(_mapper.Map<Country>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<CountryDTO>(await _countryService.ArchiveCountryAsync(Id)));
        }

        [HttpGet("ToggleStatus/{CountryId}")]
        public async Task<IActionResult> ToggleStatus(long CountryId)
        {
            IEnumerable<Country> countries = await _countryService.GetCountryByIdAsync(CountryId);
            Country country = countries.FirstOrDefault();

            if (country.Status == Enum.GetName(typeof(Status), Status.Active))
                country.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                country.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _countryService.UpdateCountryAsync(country));
        }
    }
}
