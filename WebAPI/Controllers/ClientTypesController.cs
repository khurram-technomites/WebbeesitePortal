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
    [Authorize(Roles = "Admin,Supplier,Vendor")]
    public class ClientTypesController : ControllerBase
    {
        private readonly IClientTypesService _cityService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientTypesController(IClientTypesService cityService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientTypesDTO>>(await _cityService.GetAllCitiesAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientTypesDTO>>(await _cityService.GetAllCitiesAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<ClientTypesDTO> cities = _mapper.Map<IEnumerable<ClientTypesDTO>>(await _cityService.GetCityByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClientTypesDTO Model)
        {
            /*string LogoPath = "/Images/City/";
            if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
            {
                Model.Logo = LogoPath;
            }*/

            Model.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientTypesDTO>(await _cityService.AddCityAsync(_mapper.Map<ClientTypes>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClientTypesDTO Model)
        {
            var clientIndustry = _mapper.Map<ClientTypesDTO>(await _cityService.UpdateCityAsync(_mapper.Map<ClientTypes>(Model)));
            
            return Ok(clientIndustry);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<ClientTypesDTO>(await _cityService.ArchiveCityAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<ClientTypes> cityList = await _cityService.GetCityByIdAsync(Id);
            ClientTypes city = cityList.FirstOrDefault();

            if (city.Status == Enum.GetName(typeof(Status), Status.Active))
                city.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                city.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientTypesDTO>(await _cityService.UpdateCityAsync(city)));
        }
    }
}
