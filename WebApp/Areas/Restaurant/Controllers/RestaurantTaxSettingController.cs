using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;
using HelperClasses.DTOs.Restaurant;
using WebApp.ErrorHandling;
using Newtonsoft.Json;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantTaxSettingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantTaxSettingClient _restaurantTaxSettingClient;
        private readonly IUserSessionManager _userSession;
        private readonly IRestaurantBranchClient _restaurantBranchClient;
        public RestaurantTaxSettingController(IMapper mapper, IRestaurantTaxSettingClient restaurantTaxSettingClient, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
        {
            _mapper = mapper;
            _restaurantTaxSettingClient = restaurantTaxSettingClient;
            _userSession = userSession;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;

            long restaurantId = _userSession.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<RestaurantTaxSettingViewModel>>(await _restaurantTaxSettingClient.GetAllByRestaurantIdAsync(restaurantId));
            //IEnumerable<RestaurantTaxSettingViewModel> info = _mapper.Map<IEnumerable<RestaurantTaxSettingViewModel>>(await _restaurantTaxSettingClient.GetAllAsync());
            return View(Info);

        }

        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.printerSetting = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantTaxSettingViewModel model)
        {
            try
            {
                var session = _userSession.GetUserStore();
                model.RestaurantId = _userSession.GetUserStore().Id;
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    RestaurantTaxSettingDTO Result = await _restaurantTaxSettingClient.AddRestaurantTaxSettingAsync(_mapper.Map<RestaurantTaxSettingDTO>(model));
                    if (Result != null)
                    {
                        Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId);
                    }
                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/RestaurantTaxSetting/Index",
                        message = "Record Added Successfully",
                        data = new
                        {


                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            Type = Result.RestaurantBranch.NameAsPerTradeLicense,
                            Tax = Result.TAXPercent,
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
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            var Info = _mapper.Map<RestaurantTaxSettingViewModel>(await _restaurantTaxSettingClient.GetAllByIdAsync(id));

            return View(Info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantTaxSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSession.GetUserStore().Id;
                    RestaurantTaxSettingDTO Result = await _restaurantTaxSettingClient.UpdateRestaurantTaxSettingAsync(_mapper.Map<RestaurantTaxSettingDTO>(model));

                    if (Result != null)
                    {
                        Result.RestaurantBranch = await _restaurantBranchClient.GetRestaurantBranchByIdAsync(Result.RestaurantBranchId);
                    }
                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {

                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.Name,
                            Type = Result.RestaurantBranch.NameAsPerTradeLicense,
                            TaxPercent = Result.TAXPercent,
                            Id = Result.Id,

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
                await _restaurantTaxSettingClient.DeleteRestaurantTaxSettingAsync(id);

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
            RestaurantTaxSettingViewModel printerSetting = _mapper.Map<RestaurantTaxSettingViewModel>(await _restaurantTaxSettingClient.GetAllByIdAsync(id));
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(printerSetting.RestaurantId);
            return View(printerSetting);
        }

    }
}
