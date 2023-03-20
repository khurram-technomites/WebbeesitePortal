using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Authorize(Roles = "Admin , RestaurantOwner")]
    [Route("api/Restaurant")]
    [ApiController]
    public class RestaurantUserLogManagementController : ControllerBase
    {
        private readonly IRestaurantUserLogManagementService _restaurantUserLogManagementService;
        private readonly IMapper _mapper;

        public RestaurantUserLogManagementController(IRestaurantUserLogManagementService restaurantUserLogManagementService, IMapper mapper)
        {
            _restaurantUserLogManagementService = restaurantUserLogManagementService;
            _mapper = mapper;
        }

        [HttpGet("UserLogManagement")]
        public async Task<IActionResult> GetAll()
        {
            var UserLogManagement = _mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.GetAllAsync());
            return Ok(UserLogManagement);
        }

        [HttpGet("UserLogManagement/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var UserLogManagement = _mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.GetByIdAsync(Id));
            return Ok(UserLogManagement.FirstOrDefault());
        }

        [HttpGet("{Id}/UserLogManagement")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
        {
            var UserLogManagement = _mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.GetByRestaurantIdAsync(Id));
            return Ok(UserLogManagement);
        }

        [HttpGet("UserLogManagement/ByBranch/{Id}")]
        public async Task<IActionResult> GetByBranchIdAsync(long Id)
        {
            var UserLogManagement = _mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.GetByRestaurantBranchIdAsync(Id));
            return Ok(UserLogManagement);
        }
        //[HttpGet("UserLogManagement/ServiceStaff/{Id}")]        
        //public async Task<IActionResult> GetByServiceSatffId(long Id)
        //{
        //    var UserLogManagement = _mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.GetByServiceStaffIdAsync(Id));
        //    return Ok(UserLogManagement);
        //}

        [HttpPost("UserLogManagement")]
        public async Task<IActionResult> AddUserLogManagement(RestaurantUserLogManagementDTO Model)
        {
            return Ok(_mapper.Map<RestaurantUserLogManagementDTO>(await _restaurantUserLogManagementService.AddRestaurantUserLogManagementAsync(_mapper.Map<RestaurantUserLogManagement>(Model))));
        }

        [HttpPut("UserLogManagement")]
        public async Task<IActionResult> UpdateUserLogManagement(RestaurantUserLogManagementDTO Model)
        {
            RestaurantUserLogManagement restaurantUserLogManagement = await _restaurantUserLogManagementService.GetByIdAsync(Model.Id);
            restaurantUserLogManagement=_mapper.Map(Model, restaurantUserLogManagement);

            return Ok(_mapper.Map<RestaurantUserLogManagementDTO>(await _restaurantUserLogManagementService.UpdateRestaurantUserLogManagementAsync(_mapper.Map<RestaurantUserLogManagement>(restaurantUserLogManagement))));
        }

        [HttpDelete("UserLogManagement/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantUserLogManagementDTO>>(await _restaurantUserLogManagementService.ArchiveRestaurantUserLogManagementAsync(Id)));
        }
    }
}
