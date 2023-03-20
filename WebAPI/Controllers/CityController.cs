using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Supplier")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public CityController(ICityService cityService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<CityDTO>>(await _cityService.GetAllCitiesAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterAsync()
        {
            return Ok(_mapper.Map<IEnumerable<CityDTO>>(await _cityService.GetAllCitiesAsync()));
        }

        [HttpGet("Countries/{CountryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCitiesAsync(long CountryId)
        {
            return Ok(_mapper.Map<IEnumerable<CityDTO>>(await _cityService.GetAllCitiesByCountry(CountryId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<CityDTO> cities = _mapper.Map<IEnumerable<CityDTO>>(await _cityService.GetCityByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CityDTO Model)
        {
            /*string LogoPath = "/Images/City/";
            if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
            {
                Model.Logo = LogoPath;
            }*/

            Model.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<CityDTO>(await _cityService.AddCityAsync(_mapper.Map<City>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(CityDTO Model)
        {
            string LogoPath = "/Images/City/";

            IEnumerable<City> List = await _cityService.GetCityByIdAsync(Model.Id);
            City city = List.FirstOrDefault();

            if (Model.Logo is not null && !city.Logo.Equals(Model.Logo))
            {
                if (_fTPUpload.DeleteFile(city.Logo))
                {
                    if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                    {
                        Model.Logo = LogoPath;
                    }
                }
            }

           var City = _mapper.Map(Model, city);

            return Ok(_mapper.Map<CityDTO>(await _cityService.UpdateCityAsync(_mapper.Map<City>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<CityDTO>(await _cityService.ArchiveCityAsync(Id)));
        }

        [HttpGet("ToggleStatus/{CityId}")]
        public async Task<IActionResult> ToggleStatus(long CityId)
        {
            IEnumerable<City> cityList = await _cityService.GetCityByIdAsync(CityId);
            City city = cityList.FirstOrDefault();

            if (city.Status == Enum.GetName(typeof(Status), Status.Active))
                city.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                city.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<CityDTO>(await _cityService.UpdateCityAsync(city)));
        }
    }
}
