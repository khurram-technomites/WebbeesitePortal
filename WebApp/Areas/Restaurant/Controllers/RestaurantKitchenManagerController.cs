using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
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
using HelperClasses.DTOs.RestaurantKitchenManager;
using System;
using WebApp.ViewModels.Restaurant;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantKitchenManagerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantKitchenManagerClient _restaurantKitchenManagerClient;
        private readonly IUserSessionManager _userSession;
        private readonly IRestaurantBranchClient _restaurantBranchClient;

        public RestaurantKitchenManagerController(IMapper mapper, IRestaurantKitchenManagerClient restaurantKitchenManagerClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
        {
            _mapper = mapper;
            _restaurantKitchenManagerClient = restaurantKitchenManagerClient;
            _userSession = userSession;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;

			long restaurantId = _userSession.GetUserStore().Id;
            IEnumerable<RestaurantKitchenManagerViewModel> info = _mapper.Map<IEnumerable<RestaurantKitchenManagerViewModel>>(await _restaurantKitchenManagerClient.GetAllByRestaurantIdAsync(restaurantId));
            return View(info);

        }

        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantKitchenManagerViewModel model)
        {
            try
            {
                var session = _userSession.GetUserStore();
                model.RestaurantId = _userSession.GetUserStore().Id;
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    RestaurantKitchenManagerDTO Result = await _restaurantKitchenManagerClient.AddRestaurantKitchenManagerAsync(_mapper.Map<RestaurantKitchenManagerDTO>(model));
					RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

					Result.RestaurantBranchName = branches.NameAsPerTradeLicense;

					return Json(new
                    {
                        success = true,
                        url = "/Restaurant/RestaurantKitchenManager/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            FirstName = Result.FirstName,
                            LastName = Result.LastName,
                            RestaurantBranchName = Result.RestaurantBranchName,
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
            var Info = _mapper.Map<RestaurantKitchenManagerViewModel>(await _restaurantKitchenManagerClient.GetAllByIdAsync(id));

            return View(Info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantKitchenManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSession.GetUserStore().Id;
                    RestaurantKitchenManagerDTO Result = await _restaurantKitchenManagerClient.UpdateRestaurantKitchenManagerAsync(_mapper.Map<RestaurantKitchenManagerDTO>(model));
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
                            FirstName = Result.FirstName,
                            LastName = Result.LastName,
                            RestaurantBranchName = Result.RestaurantBranchName,
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
                RestaurantKitchenManagerViewModel Result = _mapper.Map<RestaurantKitchenManagerViewModel>(await _restaurantKitchenManagerClient.ToggleActiveStatus(id));
                RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId));

                Result.RestaurantBranchName = branches.NameAsPerTradeLicense;
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        FirstName = Result.FirstName,
                        LastName = Result.LastName,
                        RestaurantBranchName = Result.RestaurantBranchName,
                        //Parent = Parent != null ? (Parent.CategoryName) : "",
                        //IsParentCategoryDeleted = category.IsParentCategoryDeleted.HasValue ? category.IsParentCategoryDeleted.Value.ToString() : bool.FalseString,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _restaurantKitchenManagerClient.DeleteRestaurantKitchenManagerAsync(id);

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
            RestaurantKitchenManagerViewModel KitchenManager = _mapper.Map<RestaurantKitchenManagerViewModel>(await _restaurantKitchenManagerClient.GetAllByIdAsync(id));
            return View(KitchenManager);
        }
    }
}
