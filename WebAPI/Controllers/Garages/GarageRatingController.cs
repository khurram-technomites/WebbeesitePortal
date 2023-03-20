using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Garage;
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

namespace WebAPI.Controllers.Garages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GarageRatingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGarageRatingService _service;
        public GarageRatingController(IMapper mapper, IGarageRatingService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<GarageRatingDTO>>(await _service.GetAllRatingForGarageAsync()));
        }
        [HttpGet("{RatingId}")]
        public async Task<IActionResult> GetByIdAsync(long RatingId)
        {
            IEnumerable<GarageRatingDTO> garageRating = _mapper.Map<IEnumerable<GarageRatingDTO>>(await _service.GetGarageRatingByIdAsync(RatingId));
            return Ok(garageRating.FirstOrDefault());
        }

        [HttpGet("ToggleStatus/{RatingId}/{status}")]
        public async Task<IActionResult> ToggleStatus(long RatingId , string status)
        {
            IEnumerable<GarageRating> garageRatings = await _service.GetGarageRatingByIdAsync(RatingId);
            GarageRating garageRating = garageRatings.FirstOrDefault();


            garageRating.Status = status;
            if (garageRating.Status == Enum.GetName(typeof(Status), Status.Approved))
            {
                garageRating.PublishedDatetime = DateTime.UtcNow.ToDubaiDateTime();
            }

            return Ok(await _service.UpdateGarageRatingAsync(garageRating));
        }
    }
}
