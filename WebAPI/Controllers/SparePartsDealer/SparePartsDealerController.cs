using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.SparePartsDealer.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SparePartDealer, Automobile Manager")]
    public class SparePartsDealerController : ControllerBase
    {
        private readonly ISparePartsDealerService _sparePartsDealerService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public SparePartsDealerController(ISparePartsDealerService sparePartsDealerService,
            IMapper mapper, IFTPUpload fTPUpload, UserManager<AppUser> userManager
            , IUserService userService)
        {
            _sparePartsDealerService = sparePartsDealerService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllSparePartsDealer(SparePartFilterDTO Filter)
        {
            return Ok(new SuccessResponse<IEnumerable<SparePartsDealerDTO>>("", _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetAllSparePartsDealerAsync(Filter))));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetAllSparePartsDealerAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSparePartsDealerById(long Id)
        {
            IEnumerable<SparePartsDealerDTO> List = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("SparePartDealer/AccountInfo/{Id}")]
        public async Task<IActionResult> GetSparePartsDealerAccountInfoById(long Id)
        {
            IEnumerable<SparePartsDealerDTO> List = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Id));
            return Ok(new { Status = "success", Data = List.FirstOrDefault() });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSparePartsDealer(SparePartsDealerDTO Model)
        {
            ErrorDetails err;
            IEnumerable<Models.SparePartsDealer> sparePartDealers = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Model.Id);

            //string sparePartDealerUser = sparePartDealers.FirstOrDefault().UserId;
            //AppUser user = await _userManager.FindByIdAsync(sparePartDealerUser);

            Model.User = null;
            if (sparePartDealers.Count() == 0)
                return Conflict(err = new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

            string LogoPath = "/Images/SparePartDealer/" + Model.NameAsPerTradeLicense + "/";

            if (!sparePartDealers.FirstOrDefault().Logo.Equals(Model.Logo))
            {
                if (_fTPUpload.DeleteFile(sparePartDealers.FirstOrDefault().Logo))
                {
                    if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                    {
                        Model.Logo = LogoPath;
                    }
                }
            }

            Models.SparePartsDealer sparePartsDealer = _mapper.Map(Model, sparePartDealers.FirstOrDefault());

            return Ok(new SuccessResponse<SparePartsDealerDTO>("", _mapper.Map<SparePartsDealerDTO>(await _sparePartsDealerService.UpdateSparePartsDealerAsync(sparePartsDealer))));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSparePartsDealerForApp(SparePartsDealerDTO Model)
        {
            ErrorDetails err;
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.SparePartsDealer> sparePartDealers = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Model.Id);

            //string sparePartDealerUser = sparePartDealers.FirstOrDefault().UserId;
            //AppUser user = await _userManager.FindByIdAsync(sparePartDealerUser);
            AppUser user = await _userManager.FindByIdAsync(sparePartDealers.FirstOrDefault().UserId);
            if (user != null && Model.User.PhoneNumber != user.PhoneNumber)
            {
                IEnumerable<AppUser> userCheckByPhone = await _userService.GetUserByNumberAndCheck(Model.User.PhoneNumber, Enum.GetName(typeof(Logins), Logins.SparePartDealer));

                if (userCheckByPhone.Any())
                    return Conflict("This phone number is used by another account");
            }

            if (sparePartDealers.Count() == 0)
                return Conflict(err = new ErrorDetails(409, "Invalid Id, No record found!", string.Empty));

            string LogoPath = "/Images/SparePartDealer/" + Model.NameAsPerTradeLicense + "/";

            
            if (!sparePartDealers.FirstOrDefault().Logo.Equals(Model.Logo))
            {
                if (_fTPUpload.DeleteFile(sparePartDealers.FirstOrDefault().Logo))
                {
                    if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
                    {
                        Model.Logo = LogoPath;
                    }
                }
            }
            Models.SparePartsDealer sparePartsDealer = _mapper.Map(Model, sparePartDealers.FirstOrDefault());
            sparePartsDealer.User = null;
            //Models.SparePartsDealer sparePartsDealer = _mapper.Map(Model, sparePartDealers.FirstOrDefault());

            SparePartsDealerDTO sparePart = _mapper.Map<SparePartsDealerDTO>(await _sparePartsDealerService.UpdateSparePartsDealerAsync(sparePartsDealer));
            AppUser AppUser = await _userManager.FindByIdAsync(UserId);
            AppUser.FirstName = sparePart.NameAsPerTradeLicense;
            AppUser.LastName = sparePart.NameAsPerTradeLicense;
            AppUser.Email = sparePart.ContactPersonEmail;

            await _userManager.UpdateAsync(AppUser);
            return Ok(new SuccessResponse<SparePartsDealerDTO>("", sparePart));
        }
        [HttpGet("{Id}/ToggleActiveStatus/{flag}")]
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason = "")
        {
            IEnumerable<Models.SparePartsDealer> List = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(Id);
            Models.SparePartsDealer sparePartsDealer = List.FirstOrDefault();

            if (sparePartsDealer.Status == Enum.GetName(typeof(Status), Status.Processing))
            {
                if (flag == false)
                {
                    sparePartsDealer.Status = Enum.GetName(typeof(Status), Status.Rejected);
                    sparePartsDealer.RejectionReason += RejectionReason + "<br />";
                }

                else if (flag == true)
                    sparePartsDealer.Status = Enum.GetName(typeof(Status), Status.Active);
            }
            else if (sparePartsDealer.Status == Enum.GetName(typeof(Status), Status.Active))
            {
                sparePartsDealer.Status = Enum.GetName(typeof(Status), Status.Inactive);
            }
            else if (sparePartsDealer.Status == Enum.GetName(typeof(Status), Status.Inactive))
            {
                sparePartsDealer.Status = Enum.GetName(typeof(Status), Status.Active);
            }

            sparePartsDealer.DealerInventorySpecifications = null;
            sparePartsDealer.SparePartsDealerDocuments = null;
            sparePartsDealer.DealerImages = null;
            sparePartsDealer.DealerSchedules = null;
            sparePartsDealer = await _sparePartsDealerService.UpdateSparePartsDealerAsync(sparePartsDealer);

            return Ok(sparePartsDealer);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartsDealerDTO>(await _sparePartsDealerService.ArchiveSparePartsDealerAsync(Id)));
        }

        [HttpPut("{SparePartDealerId}/ProfilePicture")]
        public async Task<IActionResult> UpdateProfile(long SparePartDealerId, HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model)
        {
            IEnumerable<Models.SparePartsDealer> list = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(SparePartDealerId);

            if (model.Logo is not null && model.Logo.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePartDealer/" + list.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(model.Logo, ref LogoPath))
                {
                    list.FirstOrDefault().Logo = LogoPath;
                }
            }

            Models.SparePartsDealer result = await _sparePartsDealerService.UpdateSparePartsDealerAsync(list.FirstOrDefault());
            return Ok(result.Logo);
        }

        [HttpPut("{SparePartDealerId}/Theme")]
        public async Task<IActionResult> UpdateTheme(long SparePartDealerId, HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model)
        {
            IEnumerable<Models.SparePartsDealer> list = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(SparePartDealerId);

            list.FirstOrDefault().ThemeColor = model.ThemeColor;


            Models.SparePartsDealer result = await _sparePartsDealerService.UpdateSparePartsDealerAsync(list.FirstOrDefault());
            return Ok(result.ThemeColor);
        }

        [HttpPut("{SparePartDealerId}/SecondaryLogo")]
        public async Task<IActionResult> SecondaryLogo(long SparePartDealerId, HelperClasses.DTOs.GarageCMS.ThemeColorAndLogoDTO model)
        {
            IEnumerable<Models.SparePartsDealer> list = await _sparePartsDealerService.GetSparePartsDealerByIdAsync(SparePartDealerId);

            if (!string.IsNullOrEmpty(model.SecondaryLogo))
            {
                string LogoPath = "/Images/SparePartDealer/";
                if (_fTPUpload.MoveFile(model.SecondaryLogo, ref LogoPath))
                    list.FirstOrDefault().SecondaryLogo = LogoPath;
                else
                    list.FirstOrDefault().SecondaryLogo = null;
            }

            //list.FirstOrDefault().SparePartDealerRepairSpecifications = null;
            //list.FirstOrDefault().SparePartDealerSchedules = null;

            Models.SparePartsDealer result = await _sparePartsDealerService.UpdateSparePartsDealerAsync(list.FirstOrDefault());
            return Ok(result.SecondaryLogo);
        }


    }
}
