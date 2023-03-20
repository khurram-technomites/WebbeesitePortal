using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageBlogController : ControllerBase
    {
        private readonly IGarageBlogService _service;
        private readonly IGarageService _garageService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public GarageBlogController(IGarageBlogService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _garageService = garageService;
        }

        [HttpGet("Blog")]
        public async Task<IActionResult> GetAllGarageBlog()
        {
            return Ok(_mapper.Map<IEnumerable<GarageBlogDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Blog/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageBlogDTO>>(await _service.GetGarageBlogByIdAsync(Id)));
        }

        [HttpGet("{Id}/Blog")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageBlogDTO>>(await _service.GetGarageBlogByGarageIdAsync(Id)));
        }
        [HttpGet("{Id}/Blog/Count")]
        public async Task<IActionResult> GetCountByGarageId(long Id)
        {
            return Ok(_mapper.Map<long>(await _service.GetCountByGarageIdAsync(Id)));
        }
        [HttpPost("Blog")]
        public async Task<IActionResult> Add(GarageBlogDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            Model.Slug = Slugify.GenerateSlug(Model.Title);
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageBlogDTO>(await _service.AddGarageBlogAsync(_mapper.Map<GarageBlog>(Model))));
        }

        [HttpPut("Blog")]
        public async Task<IActionResult> Update(GarageBlogDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.ImagePath) && Model.ImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageBlogDTO>(await _service.UpdateGarageBlogAsync(_mapper.Map<GarageBlog>(Model))));
        }

        [HttpDelete("Blog/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageBlogDTO>(await _service.ArchiveGarageBlogAsync(Id)));
        }

        [HttpGet("Blog/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<GarageBlog> list = await _service.GetGarageBlogByIdAsync(Id);
            GarageBlog content = list.FirstOrDefault();

            if (content.Status == Enum.GetName(typeof(Status), Status.Active))
                content.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                content.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<GarageBlogDTO>(await _service.UpdateGarageBlogAsync(content)));

        }
    }
}
