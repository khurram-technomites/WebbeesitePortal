using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Helpers;
using HelperClasses.Classes;
using System.Linq;
using System;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/Module")]
    public class ModuleController : Controller
    {
        private readonly IModuleService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public ModuleController(IModuleService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModule()
        {
            return Ok(_mapper.Map<IEnumerable<ModuleDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<ModuleDTO>>(await _service.GetModuleById(Id)));
        }

       

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ModuleDTO Model)
        {
            var module = _mapper.Map<IEnumerable<ModuleDTO>>(await _service.GetModuleByName(Model.ServiceName));
            if(module.Count()>0)
                return Conflict("Service already exists");
            Model.IsActive = true;
            Model.IsSystem = false;
            return Ok(_mapper.Map<ModuleDTO>(await _service.AddModuleAsync(_mapper.Map<Module>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ModuleDTO Model)
        {
            var _module = _mapper.Map<IEnumerable<ModuleDTO>>(await _service.GetModuleById(Model.Id)).FirstOrDefault();
            if(_module.ServiceName != Model.ServiceName)
            {
                var module = _mapper.Map<IEnumerable<ModuleDTO>>(await _service.GetModuleByName(Model.ServiceName));
                if (module.Count() > 0)
                    return Conflict("Service already exists");
            }
           
            
            return Ok(_mapper.Map<ModuleDTO>(await _service.UpdateModuleAsync(_mapper.Map<Module>(Model))));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
           


            IEnumerable<Module> modulelist = await _service.GetModuleById(Id);
            Module module = modulelist.FirstOrDefault();

            if (module.IsActive == true)
                module.IsActive = false;
            else
                module.IsActive = true;

            return Ok(_mapper.Map<ModuleDTO>(await _service.UpdateModuleAsync(module)));

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<ModuleDTO>(await _service.ArchiveModuleAsync(Id)));
        }
    }
}
