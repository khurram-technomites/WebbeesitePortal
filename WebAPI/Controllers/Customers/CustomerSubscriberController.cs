using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Subscriber")]
    [ApiController]
    public class CustomerSubscriberController : ControllerBase
    {
        private readonly IRestaurantSubscriberService _service;
        private readonly IMapper _mapper;
        public CustomerSubscriberController(IRestaurantSubscriberService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> KeepInTouch(RestaurantSubscriberDTO Model)
        {
            IEnumerable<RestaurantSubscriber> subscribers = await _service.GetSubscribersByEmailAsync(Model.Email);

            if (subscribers.Any())
                return Conflict(new ErrorDetails(409, "Already subscribed", ""));

            await _service.AddSubscriberAsync(_mapper.Map<RestaurantSubscriber>(Model));
            return Ok(new SuccessResponse<string>("Newsletter subscribed successfully ...", ""));
        }
    }
}
