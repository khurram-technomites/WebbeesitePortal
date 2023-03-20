using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantSubscriberController : ControllerBase
    {
        private readonly IRestaurantSubscriberService _subscriberService;
        private readonly IEmailService _emailService;
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantSubscriberController(IRestaurantSubscriberService subscriberService, IMapper mapper, IEmailService emailService, IRestaurantService restaurantService)
        {
            _subscriberService = subscriberService;
            _mapper = mapper;
            _emailService = emailService;
            _restaurantService = restaurantService;
        }

        [HttpGet("{RestaurantId}/Subscriber")]
        public async Task<IActionResult> GetByRestaurant(long RestaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantSubscriberDTO>>(await _subscriberService.GetSubscribersByRestaurantAsync(RestaurantId)));
        }

        [HttpDelete("Subscriber/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _subscriberService.DeleteSubscriberAsync(Id);
            return Ok();
        }

        [HttpPost("Subscriber/SendEmail")]
        public async Task<IActionResult> SendEmail([FromQuery] string Email, [FromQuery] string Body)
        {
            string UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Restaurant = await _restaurantService.GetRestaurantByUserAsync(UserID);

            IEnumerable<RestaurantSubscriber> List = await _subscriberService.GetSubscribersByEmailAsync(Email);

            if (!List.Any())
                return Conflict();

            string email = List.FirstOrDefault().Email;

            await _emailService.SendSubscriberEmail(Restaurant.FirstOrDefault().Email, email, Body);

            return Ok();
        }
    }
}
