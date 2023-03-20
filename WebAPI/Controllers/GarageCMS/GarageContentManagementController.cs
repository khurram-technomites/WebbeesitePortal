using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageContentManagementController : ControllerBase
    {
        private readonly IGarageContentManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public GarageContentManagementController(IGarageContentManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _garageService = garageService;
        }

        [HttpGet("ContentManagement")]
        public async Task<IActionResult> GetAllGarageContentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageContentManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ContentManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageContentManagementDTO>>(await _service.GetGarageContentManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/ContentManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageContentManagementDTO>>(await _service.GetGarageContentManagementByGarageIdAsync(Id)));
        }

        [HttpPost("ContentManagement")]
        public async Task<IActionResult> Add(GarageContentManagementDTO Model)
        {
            if (Model.AboutUsImage is not null && Model.AboutUsImage.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(Model.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.AboutUsImage, ref LogoPath))
                {
                    Model.AboutUsImage = LogoPath;
                }
            }

            if (Model.CEOImagePath is not null && Model.CEOImagePath.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(Model.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.CEOImagePath, ref LogoPath))
                {
                    Model.CEOImagePath = LogoPath;
                }
            }

            if (Model.InnerPagesBanner is not null && Model.InnerPagesBanner.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(Model.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.InnerPagesBanner, ref LogoPath))
                {
                    Model.InnerPagesBanner = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageContentManagementDTO>(await _service.AddGarageContentManagementAsync(_mapper.Map<GarageContentManagement>(Model))));
        }

        [HttpPut("ContentManagement")]
        public async Task<IActionResult> Update(GarageContentManagementDTO Model)
        {
            IEnumerable<GarageContentManagement> contents = await _service.GetGarageContentManagementByIdAsync(Model.Id);

            GarageContentManagement content = _mapper.Map(Model, contents.FirstOrDefault());
            content.Garage = null;
            if (Model.AboutUsImage is not null && Model.AboutUsImage.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(content.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.AboutUsImage, ref LogoPath))
                {
                    content.AboutUsImage = LogoPath;
                }
            }

            if (Model.FooterImage is not null && Model.FooterImage.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(content.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.FooterImage, ref LogoPath))
                {
                    content.FooterImage = LogoPath;
                }
            }

            if (Model.CEOImagePath is not null && Model.CEOImagePath.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(content.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.CEOImagePath, ref LogoPath))
                {
                    content.CEOImagePath = LogoPath;
                }
            }

            if (Model.InnerPagesBanner is not null && Model.InnerPagesBanner.Contains("Draft"))
            {
                IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(content.GarageId);
                string LogoPath = "/Images/Garage/" + garages.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.InnerPagesBanner, ref LogoPath))
                {
                    content.InnerPagesBanner = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageContentManagementDTO>(await _service.UpdateGarageContentManagementAsync(content)));
        }

        [HttpDelete("ContentManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageContentManagementDTO>(await _service.ArchiveGarageContentManagementAsync(Id)));
        }
    }
}
