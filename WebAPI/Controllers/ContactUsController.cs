using HelperClasses.DTOs.Authentication;
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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactUsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IBusinessSettingService _businessSettingService;
        private readonly UserManager<AppUser> _userManager;

        public ContactUsController(IEmailService emailService, IBusinessSettingService businessSettingService, UserManager<AppUser> userManager)
        {
            _businessSettingService = businessSettingService;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactEmailDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            AppUser user = await _userManager.FindByIdAsync(UserId);

            IEnumerable<BusinessSettings> List = await _businessSettingService.GetAllBusinessSettingAsync();

            if (!List.Any())
                return Conflict();

            string email = List.FirstOrDefault().Email;

            await _emailService.SendContactEmail(user.Email, user.FirstName, email, Model.Message);

            return Ok(new SuccessResponse<string>("Email sent successfully", null));
        }
    }
}