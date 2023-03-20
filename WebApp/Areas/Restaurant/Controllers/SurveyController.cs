using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Survey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using WebApp.ViewModels.Survey;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class SurveyController : Controller
    {
        private readonly ISurveyClient _client;
        private readonly IRestaurantBranchClient _branchClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public SurveyController(ISurveyClient client, IMapper mapper, IUserSessionManager userSessionManager, IRestaurantBranchClient branchClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _branchClient = branchClient;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SurveyViewModel>>(await _client.GetAllSurveyByRestaurantAsync(restaurantId));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var parent = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _branchClient.GetAllRestaurantBranchsAsync(restaurantId));
            ViewBag.RestaurantBranch = new SelectList(parent, "Id", "NameAsPerTradeLicense");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SurveyViewModel model)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.RestaurantId = restaurantId;
                model.CreationDate = DateTime.Now;


                SurveyViewModel Result = _mapper.Map<SurveyViewModel>(await _client.AddSurveyAsync(_mapper.Map<SurveyDTO>(model)));
                RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _branchClient.GetRestaurantBranchByIdAsync((long)Result.RestaurantBranchId));


                return Json(new
                {
                    success = true,
                    url = "/Restaurant/Survey/Index",
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                        //Position = Result.Position,
                        Name = Result.Name,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        RestaurantName = branches.NameAsPerTradeLicense
                    }
                });

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
            long restaurantId = _userSessionManager.GetUserStore().Id;
            SurveyViewModel model = _mapper.Map<SurveyViewModel>(await _client.GetSurveyByIdAsync(id));
            var parent = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _branchClient.GetAllRestaurantBranchsAsync(restaurantId));
            ViewBag.RestaurantBranch = new SelectList(parent, "Id", "NameAsPerTradeLicense", model.RestaurantBranchId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SurveyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SurveyViewModel Result = _mapper.Map<SurveyViewModel>(await _client.UpdateSurveyAsync(_mapper.Map<SurveyDTO>(model)));
                    RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _branchClient.GetRestaurantBranchByIdAsync((long)Result.RestaurantBranchId));

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            ID = Result.Id,
                            Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                            //Position = Result.Position,
                            Name = Result.Name,
                            Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                            RestaurantName = branches.NameAsPerTradeLicense

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
                SurveyViewModel Result = _mapper.Map<SurveyViewModel>(await _client.ToggleActiveStatus(id));
                RestaurantBranchViewModel branches = _mapper.Map<RestaurantBranchViewModel>(await _branchClient.GetRestaurantBranchByIdAsync((long)Result.RestaurantBranchId));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm:tt") : "-",
                        //Position = Result.Position,
                        Name = Result.Name,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        RestaurantName = Result.RestaurantBranch.NameAsPerTradeLicense
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
                await _client.DeleteSurveyAsync(id);

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

        public async Task<ActionResult> Details(long id)
        {

            return View(_mapper.Map<SurveyViewModel>(await _client.GetSurveyByIdAsync(id)));

        }




    }
}
