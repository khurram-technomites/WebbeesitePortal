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
    [Authorize(Roles = "Admin")]
    public class ClientSectionsController : ControllerBase
    {
        private readonly IClientSectionsService _Service;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientSectionsController(IClientSectionsService Service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _Service = Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientSectionsDTO>>(await _Service.GetAllCitiesAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientSectionsDTO>>(await _Service.GetAllCitiesAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<ClientSectionsDTO> cities = _mapper.Map<IEnumerable<ClientSectionsDTO>>(await _Service.GetCityByIdAsync(Id));
            return Ok(cities.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClientSectionsDTO Model)
        {
            /*string LogoPath = "/Images/City/";
            if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
            {
                Model.Logo = LogoPath;
            }*/

            Model.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientSectionsDTO>(await _Service.AddCityAsync(_mapper.Map<ClientSections>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClientSectionsDTO Model)
        {
            string LogoPath = "/Images/City/";

            IEnumerable<ClientSections> List = await _Service.GetCityByIdAsync(Model.Id);
            ClientSections city = List.FirstOrDefault();

           var City = _mapper.Map(Model, city);

            return Ok(_mapper.Map<ClientSectionsDTO>(await _Service.UpdateCityAsync(_mapper.Map<ClientSections>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<ClientSectionsDTO>(await _Service.ArchiveCityAsync(Id)));
        }

        [HttpGet("ToggleStatus/{CityId}")]
        public async Task<IActionResult> ToggleStatus(long CityId)
        {
            IEnumerable<ClientSections> cityList = await _Service.GetCityByIdAsync(CityId);
            ClientSections city = cityList.FirstOrDefault();

            if (city.Status == Enum.GetName(typeof(Status), Status.Active))
                city.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                city.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientSectionsDTO>(await _Service.UpdateCityAsync(city)));
        }
    }
}
