using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.RestaurantCashierStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using WebApp.ViewModels.Restaurant;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantCashierStaffController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantCashierStaffClient _client;
        private readonly IUserSessionManager _userSession;
        private readonly IRestaurantBranchClient _restaurantBranchClient;

        //[BindProperty]
        //public UserViewModel Model { get; set; }

        public RestaurantCashierStaffController(IMapper mapper, IRestaurantCashierStaffClient client, IUserSessionManager userSession, IRestaurantBranchClient restaurantBranchClient)
        {
            _mapper = mapper;
            _client = client;
            _userSession = userSession;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            PagingParameters paging = new PagingParameters();
            paging.PageNumber = 1;
            paging.PageSize = 10;
            long restaurantId = _userSession.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<RestaurantCashierStaffViewModel>>(await _client.GetAllRestaurantCashierStaffsAsync(restaurantId));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.branches = await _restaurantBranchClient.GetAllRestaurantBranchsAsync(RestaurantId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RestaurantCashierStaffViewModel model)
        {
            try
            {
                var session = _userSession.GetUserStore();
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.RestaurantId = _userSession.GetUserStore().Id;
                model.IsPrinterAllowed = false;
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    RestaurantCashierStaffDTO Result = await _client.AddRestaurantCashierStaffAsync(_mapper.Map<RestaurantCashierStaffDTO>(model));
                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/RestaurantCashierStaff/Index",
                        message = "Record Added Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                            Name = Result.FirstName,
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
            RestaurantCashierStaffViewModel RestaurantCashierStaffDTO = _mapper.Map<RestaurantCashierStaffViewModel>(await _client.GetRestaurantCashierStaffByIdAsync(id));
            return View(RestaurantCashierStaffDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantCashierStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RestaurantId = _userSession.GetUserStore().Id;
                    RestaurantCashierStaffDTO Result = await _client.UpdateRestaurantCashierStaffAsync(_mapper.Map<RestaurantCashierStaffDTO>(model));
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
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Name = Result.FirstName,
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
                RestaurantCashierStaffViewModel Result = _mapper.Map<RestaurantCashierStaffViewModel>(await _client.ToggleActiveStatus(id));
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
                        Name = Result.FirstName,
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
                await _client.DeleteRestaurantCashierStaffAsync(id);

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
            return View(_mapper.Map<RestaurantCashierStaffViewModel>(await _client.GetRestaurantCashierStaffByIdAsync(id)));
        }

        public async Task<IActionResult> RestaurantCashierStaffReport()
        {
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            var Info = _mapper.Map<IEnumerable<RestaurantCashierStaffViewModel>>(await _client.GetAllRestaurantCashierStaffsAsync(_userSession.GetUserStore().Id));
            return new CSVResult<RestaurantCashierStaffViewModel>(Info, "RestaurantCashierStaff");
        }


    }
}
