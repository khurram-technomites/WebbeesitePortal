using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
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

namespace WebAPI.Controllers.ServiceStaff
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceStaffController : ControllerBase
    {
        private readonly IServiceStaffService _serviceStaffService;
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly IGarageService _garageService;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ServiceStaffController(IServiceStaffService serviceStaffService, IMapper mapper, IUserService userService, UserManager<AppUser> userManager,
            IFTPUpload fTPUpload, ISparePartsDealerService sparePartsDealerService, IGarageService garageService)
        {
            _serviceStaffService = serviceStaffService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
            _userService = userService;
            _sparePartsDealerService = sparePartsDealerService;
            _garageService = garageService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(PagingParameters Paging)
        {
            return Ok(_mapper.Map<IEnumerable<ServiceStaffDTO>>(await _serviceStaffService.GetAllServiceStaffAsync(Paging)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ServiceStaffDTO> staff = _mapper.Map<IEnumerable<ServiceStaffDTO>>(await _serviceStaffService.GetServiceStaffByIdAsync(Id));
            ServiceStaffDTO staffModel = staff.FirstOrDefault();

            return Ok(staffModel);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            ServiceStaffDTO model = _mapper.Map<ServiceStaffDTO>(await _serviceStaffService.ArchiveServiceStaffAsync(Id));

            AppUser AppUser = await _userManager.FindByIdAsync(model.UserId);
            AppUser.IsDeleted = true;

            await _userManager.UpdateAsync(AppUser);

            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ServiceStaffDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Models.ServiceStaff> serviceStaff = await _serviceStaffService.GetServiceStaffByIdAsync(Model.Id);

            if (!string.IsNullOrEmpty(Model.Logo))
                _fTPUpload.DeleteFile(serviceStaff.FirstOrDefault().Logo);

            Models.ServiceStaff staff = _mapper.Map(Model, serviceStaff.FirstOrDefault());


            if (Model.Logo is not null && Model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/ServiceStaff/" + staff.User.UserName + "/";

                if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                {
                    staff.Logo = LogoPath;
                }
            }

            staff.User = null;
            ServiceStaffDTO staffDTO = _mapper.Map<ServiceStaffDTO>(await _serviceStaffService.UpdateServiceStaffAsync(staff));

            AppUser AppUser = await _userManager.FindByIdAsync(UserId);
            AppUser.FirstName = staffDTO.FirstName;
            AppUser.LastName = staffDTO.LastName;
            AppUser.Email = staffDTO.Email;
            AppUser.PhoneNumber = staffDTO.PhoneNumber;

            await _userManager.UpdateAsync(AppUser);

            return Ok(staff);
        }

        [HttpGet("DashboardStats")]
        public async Task<IActionResult> DashboardStats()
        {
            ServiceStaffDashboardStats stats = new()
            {
                GarageCount = await _garageService.ActiveIactiveCount(),
                SparePartDealerCount = await _sparePartsDealerService.ActiveIactiveCount()
            };

            return Ok(new SuccessResponse<ServiceStaffDashboardStats>("", stats));
        }
        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Models.ServiceStaff> List = await _serviceStaffService.GetServiceStaffByIdAsync(Id);
            Models.ServiceStaff serviceStaff = List.FirstOrDefault();

            if (serviceStaff.Status == Enum.GetName(typeof(Status), Status.Active))
                serviceStaff.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                serviceStaff.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ServiceStaffDTO>(await _serviceStaffService.UpdateServiceStaffAsync(serviceStaff)));
        }
    }
}
