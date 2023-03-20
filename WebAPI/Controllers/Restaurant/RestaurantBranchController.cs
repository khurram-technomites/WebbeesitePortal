using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
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
    public class RestaurantBranchController : ControllerBase
    {
        private readonly IRestaurantBranchService _service;
        private readonly IRestaurantService _restaurantService;
        private readonly INumberRangeService _numberRangeService;
        private readonly IMapper _mapper;

        public RestaurantBranchController(IRestaurantBranchService service, IMapper mapper, INumberRangeService numberRangeService, IRestaurantService restaurantService)
        {
            _service = service;
            _numberRangeService = numberRangeService;
            _mapper = mapper;
            _restaurantService = restaurantService;
        }

        [HttpGet("GetAll/Restaurants/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantBranchDTO>>(await _service.GetAllBranchesByRestaurant(restaurantId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<RestaurantBranchDTO> List = _mapper.Map<IEnumerable<RestaurantBranchDTO>>(await _service.GetRestaurantBranchById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(RestaurantBranchDTO Model)
        {
            IEnumerable<RestaurantBranch> isBranchExist = await _service.GetBranchByName(Model.NameAsPerTradeLicense, Model.RestaurantId);
            if (!isBranchExist.Any())
            {
                foreach(var schedule in Model.BranchSchedules)
                {
                    schedule.OpeningTime.ToDubaiDateTime();
                    schedule.ClosingTime.ToDubaiDateTime();
                }

                string numberRange = await _numberRangeService.GetNumberRangeByName("RESTAURANTBRANCH");
                Model.Slug = Slugify.GenerateSlug(Model.NameAsPerTradeLicense, numberRange);

                return Ok(_mapper.Map<RestaurantBranchDTO>(await _service.AddRestaurantBranchAsync(_mapper.Map<RestaurantBranch>(Model))));
            }

            return Conflict();
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantBranchDTO Model)
        {
            IEnumerable<RestaurantBranchDTO> isBranchExist = _mapper.Map<IEnumerable<RestaurantBranchDTO>>(await _service.GetBranchByName(Model.NameAsPerTradeLicense, Model.RestaurantId, Model.Id));
            if (!isBranchExist.Any())
            {
                IEnumerable<RestaurantBranch> branch = await _service.GetRestaurantBranchById(Model.Id);

                if (branch.FirstOrDefault().IsMainBranch && !Model.IsMainBranch)
                {
                    IEnumerable<RestaurantBranch> RestaurantBranchList = await _service.GetAllBranchesByRestaurant(branch.FirstOrDefault().RestaurantId);
                    IEnumerable<RestaurantBranch> Branches = RestaurantBranchList.Where(x => x.Id != Model.Id).ToList();

                    if (!Branches.Any())
                        return Conflict("Please add another branch before deleting this one.");

                    RestaurantBranch branch1 = Branches.FirstOrDefault();
                    branch1.IsMainBranch = true;

                    await _service.UpdateRestaurantBranchAsync(branch1);
                }

				RestaurantBranch currentBranch = _mapper.Map(Model, branch.FirstOrDefault());

                return Ok(_mapper.Map<RestaurantBranchDTO>(await _service.UpdateRestaurantBranchAsync(currentBranch)));
            }
            return Conflict();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            IEnumerable<RestaurantBranch> RestaurantBranches = await _service.GetRestaurantBranchById(Id);

            if (RestaurantBranches.FirstOrDefault().IsMainBranch)
            {
                IEnumerable<RestaurantBranch> RestaurantBranchList = await _service.GetAllBranchesByRestaurant(RestaurantBranches.FirstOrDefault().RestaurantId);
                IEnumerable<RestaurantBranch> Branches = RestaurantBranchList.Where(x => x.Id != Id).ToList();

                if (!Branches.Any())
                    return Conflict("Please add another branch before deleting this one.");

                RestaurantBranch branch = Branches.FirstOrDefault();
                branch.IsMainBranch = true;

                await _service.UpdateRestaurantBranchAsync(branch);
            }

            return Ok(_mapper.Map<RestaurantBranchDTO>(await _service.ArchiveRestaurantBranchAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<RestaurantBranch> item = await _service.GetRestaurantBranchById(Id);
            RestaurantBranch make = item.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateRestaurantBranchAsync(make));
        }

        [HttpPut("{Id}/ToggleCloseStatus")]
        public async Task<IActionResult> ToggleCloseStatus(long Id, TimeSpan? ClosingTimeSpan)
        {
            IEnumerable<RestaurantBranch> list = await _service.GetRestaurantBranchById(Id);
            RestaurantBranch Branch = list.FirstOrDefault();

            Branch.IsClose = !Branch.IsClose;

            if (Branch.IsClose && ClosingTimeSpan.HasValue)
                Branch.ClosingTimeSpan = ClosingTimeSpan;
            else if (!Branch.IsClose)
                Branch.ClosingTimeSpan = null;

            return Ok(_mapper.Map<RestaurantBranchDTO>(await _service.UpdateRestaurantBranchAsync(Branch)));
        }


        [HttpPut("{Id}/ToggleMainStatus")]
        public async Task<IActionResult> ToggleMainStatus(long Id)
        {
            IEnumerable<RestaurantBranch> list = await _service.GetRestaurantBranchById(Id);           
            RestaurantBranch Branch = list.FirstOrDefault();

            IEnumerable<RestaurantBranch> branches = await _service.GetAllBranchesByRestaurant(Branch.RestaurantId);

            Branch.IsMainBranch = !Branch.IsMainBranch;

            if (!Branch.IsMainBranch)
            {
                RestaurantBranch MainToBe = branches.Where(x => x.Id != Id).FirstOrDefault();

                if (MainToBe != default)
                {
                    MainToBe.IsMainBranch = true;
                    await _service.UpdateRestaurantBranchAsync(MainToBe);
                }
            }
            else
            {
                RestaurantBranch MainToBe = branches.Where(x => x.IsMainBranch).FirstOrDefault();

                if (MainToBe != default)
                {
                    MainToBe.IsMainBranch = false;
                    await _service.UpdateRestaurantBranchAsync(MainToBe);
                }
            }

            return Ok(_mapper.Map<RestaurantBranchDTO>(await _service.UpdateRestaurantBranchAsync(Branch)));
        }
    }
}
