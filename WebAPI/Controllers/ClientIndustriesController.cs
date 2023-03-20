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
    [Authorize(Roles = "Admin,Vendor")]
    public class ClientIndustriesController : ControllerBase
    {
        private readonly IClientIndustriesService _Service;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientIndustriesController(IClientIndustriesService Service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _Service = Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientIndustriesDTO>>(await _Service.GetAllClientIndustriesAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ClientIndustriesDTO>>(await _Service.GetAllClientIndustriesAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<ClientIndustriesDTO> industries = _mapper.Map<IEnumerable<ClientIndustriesDTO>>(await _Service.GetClientIndustriesByIdAsync(Id));
            return Ok(industries.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClientIndustriesDTO Model)
        {
            Model.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientIndustriesDTO>(await _Service.AddClientIndustriesAsync(_mapper.Map<ClientIndustries>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClientIndustriesDTO Model)
        {
            string LogoPath = "/Images/City/";

            IEnumerable<ClientIndustries> List = await _Service.GetClientIndustriesByIdAsync(Model.Id);
            ClientIndustries industry = List.FirstOrDefault();

           var Industry = _mapper.Map(Model, industry);

            return Ok(_mapper.Map<ClientIndustriesDTO>(await _Service.UpdateClientIndustriesAsync(_mapper.Map<ClientIndustries>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<ClientIndustriesDTO>(await _Service.ArchiveClientIndustriesAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<ClientIndustries> cityList = await _Service.GetClientIndustriesByIdAsync(Id);
            ClientIndustries industry = cityList.FirstOrDefault();

            if (industry.Status == Enum.GetName(typeof(Status), Status.Active))
                industry.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                industry.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ClientIndustriesDTO>(await _Service.UpdateClientIndustriesAsync(industry)));
        }
    }
}
