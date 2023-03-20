using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Partner
{
    [Route("api/Partner/Branch")]
    [ApiController]
    public class PartnerBranchController : ControllerBase
    {
        private readonly IRestaurantBranchService _restaurantBranchService;
        private readonly IMapper _mapper;

        public PartnerBranchController(IRestaurantBranchService restaurantBranchService, IMapper mapper)
        {
            _restaurantBranchService = restaurantBranchService;
            _mapper = mapper;
        }

        [HttpGet("{BranchId}/OpenStatus")]
        public async Task<IActionResult> ToggleOpenStatus(long BranchId, TimeSpan? ClosingTimeSpan)
        {
            IEnumerable<Models.RestaurantBranch> BranchList = await _restaurantBranchService.GetRestaurantBranchById(BranchId);
            Models.RestaurantBranch Branch = BranchList.FirstOrDefault();

            Branch.IsClose = !Branch.IsClose;

            if (Branch.IsClose && ClosingTimeSpan.HasValue)
                Branch.ClosingTimeSpan = ClosingTimeSpan;
            else if (!Branch.IsClose)
                Branch.ClosingTimeSpan = null;

            return Ok(new SuccessResponse<RestaurantBranchDTO>(string.Format("Branch {0} successfully", Branch.IsClose ? "Closes" : "Opens"), _mapper.Map<RestaurantBranchDTO>(await _restaurantBranchService.UpdateRestaurantBranchAsync(Branch))));
        }
    }
}
