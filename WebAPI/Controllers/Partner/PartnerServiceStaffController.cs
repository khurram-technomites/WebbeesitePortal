using AutoMapper;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Partner
{
    [Route("api/Partner/ServiceStaff")]
    [ApiController]
    [Authorize(Roles = "RestaurantServiceStaff")]
    public class PartnerServiceStaffController : ControllerBase
    {
        private readonly IRestaurantServiceStaffService _service;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public PartnerServiceStaffController(IRestaurantServiceStaffService service, IMapper mapper, IFTPUpload fTPUpload,
            UserManager<AppUser> userManager)
        {
            _service = service;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet("ByUser")]
        public async Task<IActionResult> GetByID()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<RestaurantServiceStaff> List = await _service.GetRestaurantServiceStaffByUserAsync(UserId);
            return Ok(new SuccessResponse<RestaurantServiceStaffDTO>("", _mapper.Map<RestaurantServiceStaffDTO>(List.FirstOrDefault())));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantServiceStaffDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser AppUser = await _userManager.FindByIdAsync(UserId);

            IEnumerable<RestaurantServiceStaff> serviceStaff = await _service.GetRestaurantServiceStaffByUserAsync(UserId);

            if (!string.IsNullOrEmpty(Model.Logo) && !string.IsNullOrEmpty(serviceStaff.FirstOrDefault().Logo))
                _fTPUpload.DeleteFile(serviceStaff.FirstOrDefault().Logo);

            RestaurantServiceStaff staff = _mapper.Map(Model, serviceStaff.FirstOrDefault());


            if (Model.Logo is not null && Model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/RestaurantServiceStaff/" + AppUser.UserName + "/";

                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    staff.Logo = LogoPath;
                }
            }

            staff.User = null;
            RestaurantServiceStaffDTO staffDTO = _mapper.Map<RestaurantServiceStaffDTO>(await _service.UpdateRestaurantServiceStaffAsync(staff));
            
            AppUser.FirstName = staffDTO.FirstName;
            AppUser.LastName = staffDTO.LastName;
            AppUser.Email = staffDTO.Email;
            AppUser.PhoneNumber = staffDTO.PhoneNumber;

            await _userManager.UpdateAsync(AppUser);

            return Ok(new SuccessResponse<RestaurantServiceStaffDTO>("", staffDTO));
        }
    }
}
