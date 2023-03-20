using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Vendor,GarageOwner")]
    public class ClientModulesController : Controller
    {
        private readonly IClientModulesService _clientModulesService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public ClientModulesController(IClientModulesService clientModulesService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _clientModulesService = clientModulesService;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ClientModulesDTO>>(await _clientModulesService.GetAllClientModuleAsync()));
        }
        [HttpGet("{GarageID}/ForModule")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllForModuleByGarageID(long GarageID)
        {
            var Module = _mapper.Map<IEnumerable<ClientModulesDTO>>(await _clientModulesService.GetModuleByGarageID(GarageID));
            return Ok(Module.Select( x => new LayoutModuleDTO() { 
                Blog = Module != null ?  Module.Where(x =>  x.Module.ServiceName == ModulesObject.Blogs && x.Status == "Paid" && x.Garage.Id == GarageID).Count() : 0,
                Service = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Services && x.Status == "Paid" && x.Garage.Id == GarageID).Count() : 0,
                Appoinment = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Appoinmnets && x.Status == "Paid" && x.Garage.Id == GarageID).Count() : 0,
                Project = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Project && x.Status == "Paid" && x.Garage.IsProjectAllowed && x.Garage.Id == GarageID).Count() : 0,
                Partner = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Partner && x.Status == "Paid" && x.Garage.IsPartnerAllowed && x.Garage.Id == GarageID).Count() : 0,
                Teams = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Teams && x.Status == "Paid" && x.Garage.IsTeamsAllowed && x.Garage.Id == GarageID ).Count() : 0,
                Expertise = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Expertis && x.Status == "Paid" && x.Garage.IsExpertisAllowed && x.Garage.Id == GarageID ).Count() : 0,
                Award = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Award && x.Status == "Paid" && x.Garage.IsAwardAllowed && x.Garage.Id == GarageID).Count() : 0,
                Testimonial = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Testimonial && x.Status == "Paid"  && x.Garage.Id == GarageID).Count() : 0,
                Feedback = Module != null ? Module.Where(x => x.Module.ServiceName == ModulesObject.Feedback && x.Status == "Paid" && x.Garage.Id == GarageID).Count() : 0,
            }).FirstOrDefault()) ;
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ClientModulesDTO> List = _mapper.Map<IEnumerable<ClientModulesDTO>>(await _clientModulesService.GetClientModuleByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("ClientId/{ClientId}")]
        public async Task<IActionResult> GetByClientId(long ClientId)
        {
            IEnumerable<ClientModulesDTO> List = _mapper.Map<IEnumerable<ClientModulesDTO>>(await _clientModulesService.GetClientModuleByClientIdAsync(ClientId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientModulesDTO Model)
        {
            return Ok(_mapper.Map<ClientModulesDTO>(await _clientModulesService.AddClientModuleAsync(_mapper.Map<ClientModules>(Model))));
        }
        [HttpPost("Range")]
        public async Task<IActionResult> AddRange(IEnumerable<ClientModulesDTO> Model)
        {
            return Ok(_mapper.Map<IEnumerable<ClientModulesDTO>>(await _clientModulesService.AddClientModuleRangeAsync(_mapper.Map<IEnumerable<ClientModules>>(Model))));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _clientModulesService.ArchiveClientModuleAsync(Id);
            return Ok();
        }
    }
}
