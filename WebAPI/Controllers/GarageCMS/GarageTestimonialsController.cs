using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageTestimonialsController : ControllerBase
    {
        private readonly IGarageTestimonialsService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        private readonly IGarageService _garageService;
        public GarageTestimonialsController(IGarageTestimonialsService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _garageService = garageService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("Testimonials")]
        public async Task<IActionResult> GetAllGarageTestimonials()
        {
            return Ok(_mapper.Map<IEnumerable<GarageTestimonialsDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Testimonials/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageTestimonialsDTO>>(await _service.GetGarageTestimonialsByIdAsync(Id)));
        }

        [HttpGet("{Id}/Testimonials")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageTestimonialsDTO>>(await _service.GetGarageTestimonialsByGarageIdAsync(Id)));
        }
        [HttpGet("Count/{Id}/Testimonials")]
        public async Task<IActionResult> GetAllCountByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageTestimonialsDTO>>(await _service.GetCountByGarageIdAsync(Id)));
        }
        [HttpPost("Testimonials")]
        public async Task<IActionResult> Add(GarageTestimonialsDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.CustomerImage))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CustomerImage, ref LogoPath))
                {
                    Model.CustomerImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageTestimonialsDTO>(await _service.AddGarageTestimonialsAsync(_mapper.Map<GarageTestimonials>(Model))));
        }

        [HttpPut("Testimonials")]
        public async Task<IActionResult> Update(GarageTestimonialsDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.CustomerImage) && Model.CustomerImage.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.CustomerImage, ref LogoPath))
                {
                    Model.CustomerImage = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageTestimonialsDTO>(await _service.UpdateGarageTestimonialsAsync(_mapper.Map<GarageTestimonials>(Model))));
        }

        [HttpDelete("Testimonials/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageTestimonialsDTO>(await _service.ArchiveGarageTestimonialsAsync(Id)));
        }

        [HttpGet("Testimonials/{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
             IEnumerable<GarageTestimonials> testimonials = await _service.GetGarageTestimonialsByIdAsync(Id);
            GarageTestimonials testimonial = testimonials.FirstOrDefault();
            if (testimonial.ShowOnWebsite == true)
                testimonial.ShowOnWebsite = false;
            else
                testimonial.ShowOnWebsite = true;

            return Ok(_mapper.Map<GarageTestimonialsDTO>(await _service.UpdateGarageTestimonialsAsync(_mapper.Map<GarageTestimonials>(testimonial))));
        }

    }
}
