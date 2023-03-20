using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class RestaurantWaiterController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantWaiterClient _RestaurantWaiterClient;
        private readonly IUserSessionManager _userSession;
        private readonly IRestaurantBranchClient _restaurantBranchClient;


        public RestaurantWaiterController(IMapper mapper, IRestaurantWaiterClient RestaurantWaiterClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
        {
            _mapper = mapper;
            _RestaurantWaiterClient = RestaurantWaiterClient;
            _userSession = userSession;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSession.GetUserStore().Id;
            IEnumerable<RestaurantWaiterViewModel> info = _mapper.Map<IEnumerable<RestaurantWaiterViewModel>>(await _RestaurantWaiterClient.GetAllByRestaurantIdAsync(restaurantId));
            return View(info);
        }

        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.printerSetting = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantWaiterViewModel model)
        {

            try
            {
                var session = _userSession.GetUserStore();
                model.RestaurantId = _userSession.GetUserStore().Id;
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    RestaurantWaiterDTO Result = await _RestaurantWaiterClient.AddRestaurantWaiterAsync(_mapper.Map<RestaurantWaiterDTO>(model));
					RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId));

					Result.RestaurantBranchName = branches.NameAsPerTradeLicense;
					return Json(new
                    {
                        success = true,
                        url = "/Restaurant/RestaurantWaiter/Index",
                        message = "Record Added Successfully",
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
            var Info = _mapper.Map<RestaurantWaiterViewModel>(await _RestaurantWaiterClient.GetAllByIdAsync(id));

            return View(Info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantWaiterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSession.GetUserStore().Id;
                    RestaurantWaiterDTO Result = await _RestaurantWaiterClient.UpdateRestaurantWaiterAsync(_mapper.Map<RestaurantWaiterDTO>(model));
					RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _restaurantBranchClient.GetRestaurantBranchByIdAsync(model.RestaurantBranchId));

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _RestaurantWaiterClient.DeleteRestaurantWaiterAsync(id);

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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                RestaurantWaiterViewModel Result = _mapper.Map<RestaurantWaiterViewModel>(await _RestaurantWaiterClient.ToggleActiveStatus(id));
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
        public async Task<IActionResult> Details(long id)
        {
            RestaurantWaiterViewModel KitchenManager = _mapper.Map<RestaurantWaiterViewModel>(await _RestaurantWaiterClient.GetAllByIdAsync(id));
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(KitchenManager.RestaurantId);
            return View(KitchenManager);

        }
    }
}
