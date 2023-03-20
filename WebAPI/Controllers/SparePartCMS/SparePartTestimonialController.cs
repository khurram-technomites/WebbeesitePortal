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
    public class SparePartTestimonialController : ControllerBase
    {
        private readonly ISparePartTestimonialService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartTestimonialController(ISparePartTestimonialService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("Testimonials")]
        public async Task<IActionResult> GetAllGarageTestimonials()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTestimonialDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Testimonials/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTestimonialDTO>>(await _service.GetSparePartTestimonialByIdAsync(Id)));
        }

        [HttpGet("{Id}/Testimonials")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTestimonialDTO>>(await _service.GetSparePartTestimonialBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("Testimonials")]
        public async Task<IActionResult> Add(SparePartTestimonialDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.CustomerImage))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CustomerImage, ref LogoPath))
                {
                    Model.CustomerImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartTestimonialDTO>(await _service.AddSparePartTestimonialAsync(_mapper.Map<SparePartTestimonial>(Model))));
        }

        [HttpPut("Testimonials")]
        public async Task<IActionResult> Update(SparePartTestimonialDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.CustomerImage) && Model.CustomerImage.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CustomerImage, ref LogoPath))
                {
                    Model.CustomerImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartTestimonialDTO>(await _service.UpdateSparePartTestimonialAsync(_mapper.Map<SparePartTestimonial>(Model))));
        }

        [HttpDelete("Testimonials/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartTestimonialDTO>(await _service.ArchiveSparePartTestimonialAsync(Id)));
        }

        [HttpGet("Testimonials/{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SparePartTestimonial> testimonials = await _service.GetSparePartTestimonialByIdAsync(Id);
            SparePartTestimonial testimonial = testimonials.FirstOrDefault();
            if (testimonial.ShowOnWebsite == true)
                testimonial.ShowOnWebsite = false;
            else
                testimonial.ShowOnWebsite = true;

            return Ok(_mapper.Map<SparePartTestimonialDTO>(await _service.UpdateSparePartTestimonialAsync(_mapper.Map<SparePartTestimonial>(testimonial))));
        }
    }
}
