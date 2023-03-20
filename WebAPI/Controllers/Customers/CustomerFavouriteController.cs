using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer/Favourite")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerFavouriteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerFavouriteBranchesService _service;
        private readonly ICustomerService _customerService;
        public CustomerFavouriteController(IMapper mapper, ICustomerFavouriteBranchesService service, ICustomerService customerService)
        {
            _mapper = mapper;
            _service = service;
            _customerService = customerService;
        }

        [HttpPost("Branch/{BranchId}")]
        public async Task<IActionResult> AddRestaurant(long BranchId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.Customer> Customer = await _customerService.GetByUserIdAsync(UserId);

            if (_service.GetByUserAndBranch(Customer.FirstOrDefault().Id, BranchId).Result.Any())
                return Conflict(new ErrorDetails(409, "Restaurant already in favourites", ""));

            CustomerFavouriteBranches favouriteBranches = new()
            {
                BranchId = BranchId,
                CustomerId = Customer.FirstOrDefault().Id
            };

            CustomerFavouriteBranches result = await _service.AddFavouriteBranch(favouriteBranches);

            return Ok(new SuccessResponse<string>("Restaurant added to favourities", ""));
        }

        [HttpDelete("Branch/{BranchId}")]
        public async Task<IActionResult> RemoveRestaurant(long BranchId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.Customer> Customer = await _customerService.GetByUserIdAsync(UserId);

            await _service.DeleteFavouriteBranch(Customer.FirstOrDefault().Id, BranchId);

            return Ok(new SuccessResponse<string>("Restaurant removed from favourities", ""));
        }

        [HttpPost("Branches")]
        public async Task<IActionResult> GetAll(RestaurantFilter Filter)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.Customer> Customer = await _customerService.GetByUserIdAsync(UserId);

            return Ok(new SuccessResponse<IEnumerable<RestaurantCardResponseDTO>>("", _service.GetAllByCustomer(Filter, Customer.FirstOrDefault().Id)));
        }
    }
}
