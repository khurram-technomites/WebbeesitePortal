using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantContentManagementController : Controller
    {
        private readonly IRestaurantContentManagementService _restaurantContentService;
        private readonly IMapper _mapper;
        private readonly IRestaurantService _restaurantService;
        private readonly IFTPUpload _fTPUpload;
        public RestaurantContentManagementController(IRestaurantContentManagementService restaurantContentService, IMapper mapper, IRestaurantService restaurantService, IFTPUpload fTPUpload)
        {
            _restaurantContentService = restaurantContentService;
            _mapper = mapper;
            _restaurantService = restaurantService;
            _fTPUpload = fTPUpload;
        }
        [HttpGet]
        public async Task<IActionResult> GetRestaurantContentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantContentManagementDTO>>(await _restaurantContentService.GetAllAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetRestaurantContentManagementByID(long Id)
        {
            IEnumerable<RestaurantContentManagementDTO> list = _mapper.Map<IEnumerable<RestaurantContentManagementDTO>>(await _restaurantContentService.GetRestaurantContentManagementByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpGet("Restaurant/{RestaurantId}")]
        public async Task<IActionResult> GetRestaurantContentManagementByRestaurantID(long RestaurantId)
        {
            IEnumerable<RestaurantContentManagementDTO> list = _mapper.Map<IEnumerable<RestaurantContentManagementDTO>>(await _restaurantContentService.GetRestaurantContentManagementByRestaurantIdAsync(RestaurantId));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(RestaurantContentManagementDTO model)
        {
            return Ok(_mapper.Map<RestaurantContentManagementDTO>(await _restaurantContentService.AddRestaurantContentManagementAsync(_mapper.Map<RestaurantContentManagement>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutRestaurantContentUpdateAsync(RestaurantContentManagementDTO model)
        {
            IEnumerable<RestaurantContentManagement> list = await _restaurantContentService.GetRestaurantContentManagementByIdAsync(model.Id);
            RestaurantContentManagement setting = list.FirstOrDefault();
            RestaurantContentManagement restaurantContent = _mapper.Map(model, setting);

            return Ok(_mapper.Map<RestaurantContentManagementDTO>(await _restaurantContentService.UpdateRestaurantContentManagementAsync(_mapper.Map<RestaurantContentManagement>(restaurantContent))));
        }
        [HttpGet("Policy/{PolicyName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPolicy(string PolicyName)
        {
            string origin = Request.Headers["origin"];

            if (string.IsNullOrEmpty(origin))
                return Conflict(new ErrorDetails(409, "Invalid origin", ""));

            IEnumerable<LandingPageResponseDTO> resultList = _restaurantService.GetRestaurantDetailsByOrigin(origin);

            IEnumerable<RestaurantContentManagement> Policies = await _restaurantContentService.GetRestaurantContentManagementByRestaurantIdAsync(resultList.FirstOrDefault().RestaurantId);

            if (PolicyName.ToLower().Replace(" ", "") == "privacypolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().PrivacyPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "deliverypolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().DeliveryPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "returnpolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().ReturnPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "termsandcondition")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().TermsAndConditions));

            return Conflict(new ErrorDetails(409, "Invalid Policy", null));

        }

        [HttpPut("Footer")]
        public async Task<IActionResult> PostFooter(RestaurantContentManagementDTO model)
        {
            IEnumerable<RestaurantContentManagement> contents = await _restaurantContentService.GetRestaurantContentManagementByRestaurantIdAsync(model.RestaurantId);

            RestaurantContentManagement content = contents.FirstOrDefault();

            if (!string.IsNullOrEmpty(content.FooterImage))
                _fTPUpload.DeleteFile(content.FooterImage);

            string LogoPath = "/Images/Restaurant/Footer";
            if (_fTPUpload.MoveFile(model.FooterImage, ref LogoPath))
            {
                content.FooterImage = LogoPath;
            }

            return Ok(_mapper.Map<RestaurantContentManagementDTO>(await _restaurantContentService.UpdateRestaurantContentManagementAsync(content)));
        }
    }
}
