using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Emails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Contact")]
    [ApiController]
    public class CustomerContactController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IBusinessSettingService _businessSettingService;
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly IRestaurantService _restaurantService;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _salesEmail;
        private readonly ILogger<CustomerContactController> _logger;

        public CustomerContactController(IEmailService emailService, IBusinessSettingService businessSettingService, UserManager<AppUser> userManager,
            IRestaurantService restaurantService, IIntegrationSettingService integrationSettingService,
            ILogger<CustomerContactController> logger)
        {
            _businessSettingService = businessSettingService;
            _userManager = userManager;
            _emailService = emailService;
            _restaurantService = restaurantService;
            _integrationSettingService = integrationSettingService;
            _salesEmail = integrationSettingService.GetAllAsync().Result.FirstOrDefault().SalesEmail;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactUsMessage(ContactUsDTO Model)
        {
            await SendContactUsAsync(Model);

            return Ok(new SuccessResponse<string>("Email sent successfully", string.Empty));
        }
        private async Task SendContactUsAsync(ContactUsDTO Model)
        {
            try
            {
                await _emailService.SendContactEmailByTemplate(new GeneralEmailDTO()
                {
                    Name = Model.Name,
                    Email = Model.Email,
                    HTMLBody = Model.Message,
                    ButtonText = Model.Subject,
                }, "Webeesite Contact Us", _salesEmail);

                _logger.LogInformation("Contact us Email Sent Successfully to " + _salesEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Contact Us Email Failed for " + _salesEmail + " with message: " +
                                  ex.Message);
            }
        }

        [HttpPost("Restaurant/{RestaurantId}")]
        public async Task<IActionResult> SendMessageToRestaurant(long RestaurantId, ContactEmailDTO Model)
        {
            string Name = Model.FirstName + " " + Model.LastName;
            IEnumerable<Models.Restaurant> List = await _restaurantService.GetRestaurantByIdAsync(RestaurantId);

            if (!List.Any())
                return Conflict();

            string email = List.FirstOrDefault().Email;

            await _emailService.SendContactEmail(Model.Email, Name, email, Model.Message);

            return Ok(new SuccessResponse<string>("Email sent successfully", null));
        }
    }
}
