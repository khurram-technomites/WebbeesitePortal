using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;
using HelperClasses.DTOs.Restaurant;
using WebApp.ErrorHandling;
using Newtonsoft.Json;
using WebAPI.Models;
using HelperClasses.Classes;
using System;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class RestaurantManagerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantManagerClient _restaurantManagerClient;
        private readonly IUserSessionManager _userSession;
        private readonly IRestaurantBranchClient _restaurantBranchClient;


        public RestaurantManagerController(IMapper mapper, IRestaurantManagerClient restaurantManagerClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
        {
            _mapper = mapper;
            _restaurantManagerClient = restaurantManagerClient;
            _userSession = userSession;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSession.GetUserStore().Id;
            IEnumerable<RestaurantManagerViewModel> info = _mapper.Map<IEnumerable<RestaurantManagerViewModel>>(await _restaurantManagerClient.GetAllByRestaurantIdAsync(restaurantId));
            return View(info);

        }

        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.printerSetting = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantManagerViewModel model)
        {

            try
            {
                var session = _userSession.GetUserStore();
                model.RestaurantId = _userSession.GetUserStore().Id;
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    RestaurantManagerDTO Result = await _restaurantManagerClient.AddRestaurantManagerAsync(_mapper.Map<RestaurantManagerDTO>(model));
                    RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

                    Result.RestaurantBranchName = branches.NameAsPerTradeLicense;

                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/RestaurantManager/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            RestaurantBranchName = Result.RestaurantBranchName,
                            Contact = Result.Contact,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false

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
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            var Info = _mapper.Map<RestaurantManagerViewModel>(await _restaurantManagerClient.GetAllByIdAsync(id));

            return View(Info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSession.GetUserStore().Id;
                    RestaurantManagerDTO Result = await _restaurantManagerClient.UpdateRestaurantManagerAsync(_mapper.Map<RestaurantManagerDTO>(model));
                    RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

                    Result.RestaurantBranchName = branches.NameAsPerTradeLicense;

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            Name = Result.Name,
                            RestaurantBranchName = Result.RestaurantBranchName,
                            Contact = Result.Contact,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false

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
                RestaurantManagerViewModel Result = _mapper.Map<RestaurantManagerViewModel>(await _restaurantManagerClient.ToggleActiveStatus(id));
                RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

                Result.RestaurantBranchName = branches.NameAsPerTradeLicense;

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        Name = Result.Name,
                        RestaurantBranchName = Result.RestaurantBranchName,
                        Contact = Result.Contact,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }

                }); ;
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
                await _restaurantManagerClient.DeleteRestaurantManagerAsync(id);

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
            RestaurantManagerViewModel KitchenManager = _mapper.Map<RestaurantManagerViewModel>(await _restaurantManagerClient.GetAllByIdAsync(id));
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(KitchenManager.RestaurantId);

            return View(KitchenManager);

        }
    }
}
