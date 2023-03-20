using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , RestaurantOwner")]
    public class RestaurantRatingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRatingService _service;
        public RestaurantRatingController(IMapper mapper, IRestaurantRatingService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantRatingDTO>>(await _service.GetAllRatingForRestaurantAsync()));
        }
        [HttpGet("Status/{Status}/{RestaurantId}")]
        public async Task<IActionResult> GetAllByStatusAsync(String Status , long RestaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantRatingDTO>>(await _service.GetAllRatingByStatusAsync(Status , RestaurantId)));
        }
        [HttpGet("{RatingId}")]
        public async Task<IActionResult> GetByIdAsync(long RatingId)
        {
            IEnumerable<RestaurantRatingDTO> restaurantRating = _mapper.Map<IEnumerable<RestaurantRatingDTO>>(await _service.GetRestaurantRatingByIdAsync(RatingId));
            return Ok(restaurantRating.FirstOrDefault());
        }
        [HttpGet("ByResataurantId/{RestaurantId}")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long RestaurantId)
        {
            IEnumerable<RestaurantRatingDTO> restaurantRating = _mapper.Map<IEnumerable<RestaurantRatingDTO>>(await _service.GetRestaurantRatingByRestaurantIdAsync(RestaurantId));
            return Ok(restaurantRating);
        }

        [HttpGet("ToggleStatus/{RatingId}/{status}")]
        public async Task<IActionResult> ToggleStatus(long RatingId , string status)
        {
            IEnumerable<RestaurantRating> restaurantRatings = await _service.GetRestaurantRatingByIdAsync(RatingId);
            RestaurantRating restaurantRating = restaurantRatings.FirstOrDefault();

            restaurantRating.Status = status;
            if (restaurantRating.Status == "Approved")
            {
                restaurantRating.PublishedDatetime = DateTime.UtcNow.ToDubaiDateTime();
            }

            return Ok(await _service.UpdateRestaurantRatingAsync(restaurantRating));
        }
        [HttpPut]
        public async Task<IActionResult> Put(RestaurantRatingDTO Model)
        {

            if (Model.ShowOnWebsite == true)
                Model.ShowOnWebsite = false;
            else
                Model.ShowOnWebsite = true;
            return Ok(_mapper.Map<RestaurantRatingDTO>(await _service.UpdateRestaurantRatingAsync(_mapper.Map<RestaurantRating>(Model))));
        }
    }
}
