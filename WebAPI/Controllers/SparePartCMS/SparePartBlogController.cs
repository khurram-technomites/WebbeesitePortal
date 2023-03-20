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
using System;
using WebAPI.Helpers;
using WebAPI.Services.Domains;
using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using System.Linq;
using HelperClasses.Classes;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartBlogController : ControllerBase
    {
        private readonly ISparePartBlogService _service;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public SparePartBlogController(ISparePartBlogService service, ISparePartsDealerService sparePartsService, IMapper mapper, IUserService userService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _sparePartsService = sparePartsService;
            _mapper = mapper;
            _userService = userService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("Blog")]
        public async Task<IActionResult> GetAllGarageBlog()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBlogDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Blog/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBlogDTO>>(await _service.GetSparePartBlogByIdAsync(Id)));
        }

        [HttpGet("{Id}/Blog")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBlogDTO>>(await _service.GetSparePartBlogBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("Blog")]
        public async Task<IActionResult> Add(SparePartBlogDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            Model.Slug = Slugify.GenerateSlug(Model.Title);
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartBlogDTO>(await _service.AddSparePartBlogAsync(_mapper.Map<SparePartBlog>(Model))));
        }

        [HttpPut("Blog")]
        public async Task<IActionResult> Update(SparePartBlogDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.ImagePath) && Model.ImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartBlogDTO>(await _service.UpdateSparePartBlogAsync(_mapper.Map<SparePartBlog>(Model))));
        }

        [HttpDelete("Blog/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartBlogDTO>(await _service.ArchiveSparePartBlogAsync(Id)));
        }

        [HttpGet("Blog/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SparePartBlog> list = await _service.GetSparePartBlogByIdAsync(Id);
            SparePartBlog content = list.FirstOrDefault();

            if (content.Status == Enum.GetName(typeof(Status), Status.Active))
                content.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                content.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<SparePartBlogDTO>(await _service.UpdateSparePartBlogAsync(content)));

        }
    }
}
