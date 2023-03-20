using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]

    public class AggregatorController : Controller
    {
        private readonly IAggregatorClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IRestaurantBranchClient _restaurantBranchClient;


        public AggregatorController(IAggregatorClient client, IMapper mapper, IUserSessionManager userSessionManager, IRestaurantBranchClient restaurantBranchClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {

            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;

            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<AggregatorViewModel>>(await _client.GetAggregatorByRestaurantIdAsync(restaurantId));
            //IEnumerable<RestaurantTaxSettingViewModel> info = _mapper.Map<IEnumerable<RestaurantTaxSettingViewModel>>(await _restaurantTaxSettingClient.GetAllAsync());
            return View(Info);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSessionManager.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AggregatorViewModel model)
        {
            try
            {
                var session = _userSessionManager.GetUserStore();
                model.RestaurantId = _userSessionManager.GetUserStore().Id;
                //model.Status = Enum.GetName(typeof(Status), Status.Inactive);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    AggregatorDTO Result = await _client.AddAggregatorAsync(_mapper.Map<AggregatorDTO>(model));

                    if (Result != null)
                    {
                        Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId);
                    }
                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/Aggregator/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            Branch = Result.RestaurantBranch.NameAsPerTradeLicense,
                            Commission = Result.Commission,
                            Taxpercent = Result.TAXPercent,
                            Status = Result.Status == Enum.GetName(typeof(Status), Status.Inactive),
                            ID = Result.Id,
                        }
                    });


                }
                else
                {
                    message = "Please fill the form properly ...";
                }
                return Json(new { success = false, message = message });
            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Oops! Something went wrong. Please try later."
                });
            }


        }


        public async Task<IActionResult> Edit(long id)
        {
            long RestaurantId = _userSessionManager.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            var Info = _mapper.Map<IEnumerable<AggregatorViewModel>>(await _client.GetAggregatorByIdAsync(id));
            return View(Info.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AggregatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSessionManager.GetUserStore().Id;
                    AggregatorDTO Result = await _client.UpdateAggregatorAsync(_mapper.Map<AggregatorDTO>(model));
                    Result.Status = model.Status;
                    if (Result != null)
                    {
                        Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId);
                    }
                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/Aggregator/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            Branch = Result.RestaurantBranch.NameAsPerTradeLicense,
                            Commission = Result.Commission,
                            Taxpercent = Result.TAXPercent,
                            Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                            ID = Result.Id,
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        success = false,
                        message = err.Message
                    });
                }

            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                AggregatorViewModel Result = _mapper.Map<AggregatorViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        Branch = Result.RestaurantBranch.NameAsPerTradeLicense,
                        Commission = Result.Commission,
                        Taxpercent = Result.TAXPercent,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                    }

                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteAggregatorAsync(id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }

        public async Task<IActionResult> Details(long id)
        {
            var Info = _mapper.Map<IEnumerable<AggregatorViewModel>>(await _client.GetAggregatorByIdAsync(id));
            return View(Info.FirstOrDefault());
        }


    }
}
