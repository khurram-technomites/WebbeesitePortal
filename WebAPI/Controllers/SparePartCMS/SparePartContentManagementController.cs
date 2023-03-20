using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services.Domains;
using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartContentManagementController : ControllerBase
    {
        private readonly ISparePartContentManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public SparePartContentManagementController(ISparePartContentManagementService service, IMapper mapper, IUserService userService, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("ContentManagement")]
        public async Task<IActionResult> GetAllGarageContentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartContentManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ContentManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartContentManagementDTO>>(await _service.GetSparePartContentManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/ContentManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartContentManagementDTO>>(await _service.GetSparePartContentManagementBySparePartIdAsync(Id)));
        }

        [HttpPost("ContentManagement")]
        public async Task<IActionResult> Add(SparePartContentManagementDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (Model.AboutUsImage is not null && Model.AboutUsImage.Contains("Draft"))
            {
                //IEnumerable<Garage> garages = await _garageService.GetGarageByIdAsync(Model.GarageId);

                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.AboutUsImage, ref LogoPath))
                {
                    Model.AboutUsImage = LogoPath;
                }
            }

            if (Model.CEOImagePath is not null && Model.CEOImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.CEOImagePath, ref LogoPath))
                {
                    Model.CEOImagePath = LogoPath;
                }
            }

            if (Model.InnerBanner is not null && Model.InnerBanner.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(Model.InnerBanner, ref LogoPath))
                {
                    Model.InnerBanner = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartContentManagementDTO>(await _service.AddSparePartContentManagementAsync(_mapper.Map<SparePartContentManagement>(Model))));
        }

        [HttpPut("ContentManagement")]
        public async Task<IActionResult> Update(SparePartContentManagementDTO Model)
        {
            IEnumerable<SparePartContentManagement> contents = await _service.GetSparePartContentManagementByIdAsync(Model.Id);

            SparePartContentManagement content = _mapper.Map(Model, contents.FirstOrDefault());

            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (content.AboutUsImage is not null && content.AboutUsImage.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(content.AboutUsImage, ref LogoPath))
                {
                    content.AboutUsImage = LogoPath;
                }
            }

            if (content.FooterImage is not null && content.FooterImage.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(content.FooterImage, ref LogoPath))
                {
                    content.FooterImage = LogoPath;
                }
            }

            if (content.CEOImagePath is not null && content.CEOImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(content.CEOImagePath, ref LogoPath))
                {
                    content.CEOImagePath = LogoPath;
                }
            }

            if (content.InnerBanner is not null && content.InnerBanner.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";

                if (_fTPUpload.MoveFile(content.InnerBanner, ref LogoPath))
                {
                    content.InnerBanner = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartContentManagementDTO>(await _service.UpdateSparePartContentManagementAsync(content)));
        }

        [HttpDelete("ContentManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartContentManagementDTO>(await _service.ArchiveSparePartContentManagementAsync(Id)));
        }
    }
}
