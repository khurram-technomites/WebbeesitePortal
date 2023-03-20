using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebApp.ErrorHandling;
using WebApp.Helpers;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantBranchController : Controller
    {
        private readonly IRestaurantBranchClient _client;
        private readonly IRestaurantBranchScheduleClient _branchSchedule;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ICountryClient _countryClient;
        private readonly ICityClient _cityClient;
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;
        public RestaurantBranchController(IRestaurantBranchClient client, IMapper mapper, IUserSessionManager userSessionManager, IFileUpload fileUpload, ICountryClient countryClient, IRestaurantBranchScheduleClient branchSchedule, ICityClient cityClient)
        {
            _client = client;
            _cityClient = cityClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _fileUpload = fileUpload;
            _countryClient = countryClient;
            _branchSchedule = branchSchedule;
        }

        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _client.GetAllRestaurantBranchsAsync(restaurantId));
            return View(Info);
        }

        public IActionResult Create()
        {
            return View();
        }


        public IActionResult GenerateQR(long Id)
        {
            return View(Id);
        }

        public async Task<IActionResult> GenerateQRImage(long Id)
        {
            MemoryStream memoryStream = new();
            RestaurantBranchDTO branch = await _client.GetRestaurantBranchByIdAsync(Id);

            QRGenerator qR = new();

            System.IO.Stream stream = qR.GenerateQR(Id.ToString());

            await stream.CopyToAsync(memoryStream);

            return Json(new
            {
                data = memoryStream.ToArray()
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(RestaurantBranchViewModel model)
        {
            try
            {
                long restaurantId = _userSessionManager.GetUserStore().Id;
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.RestaurantId = restaurantId;
                model.CountryId = null;
                model.CityId = null;

                RestaurantBranchDTO Result = await _client.AddRestaurantBranchAsync(_mapper.Map<RestaurantBranchDTO>(model));

                //var Parent = RestaurantBranch.ParentRestaurantBranchID.HasValue ? _RestaurantBranchService.GetRestaurantBranch((long)RestaurantBranch.ParentRestaurantBranchID) : null;
                return Json(new
                {
                    success = true,
                    url = "/Restaurant/RestaurantBranch/Edit/" + Result.Id,
                    message = "Record Added Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        Name = Result.NameAsPerTradeLicense,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                    }
                });


                //}
                //else
                //{
                //	message = "Please fill the form properly ...";
                //}
                //return Json(new { success = false, message = message });
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

        public IActionResult Schedule(long Id)
        {
            ViewBag.BranchId = Id;
            return View();
        }

        public async Task<IActionResult> DeleteSchedule(long Id)
        {
            await _branchSchedule.DeleteRestaurantBranchScheduleAsync(Id);

            return Json(new
            {
                success = true,
                message = "Schedule Deleted Successfully"
            });
        }

        public async Task<IActionResult> GetSchedule(long Id)
        {
            var schedule = await _branchSchedule.GetAllRestaurantBranchSchedulesAsync(Id);

            if (schedule.Any())
            {
                return Json(new
                {
                    success = true,
                    data = schedule,
                    message = "Success"
                });
            }

            return Json(new
            {
                success = false,
                message = "Something went wrong.. !"
            });

        }

        public async Task<IActionResult> SetSchedule(List<RestaurantBranchScheduleViewModel> model)
        {
            if (model.Any())
            {
                foreach (var item in model)
                {
                    if (item.Id != 0)
                    {
                        await _branchSchedule.UpdateRestaurantBranchScheduleAsync(_mapper.Map<RestaurantBranchScheduleDTO>(item));
                    }
                    else
                    {
                        await _branchSchedule.AddRestaurantBranchScheduleAsync(_mapper.Map<RestaurantBranchScheduleDTO>(item));
                    }

                }

                return Json(new
                {
                    success = true,
                    message = "Schedule Successfully Added.."
                });
            }

            return Json(new
            {
                success = false,
                message = "Record Updated Successfully"
            });

        }

        public async Task<IActionResult> Edit(long id)
        {

            RestaurantBranchViewModel RestaurantBranch = _mapper.Map<RestaurantBranchViewModel>(await _client.GetRestaurantBranchByIdAsync(id));
            var country = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster());
            ViewBag.CountryId = new SelectList(country, "Id", "Name", RestaurantBranch.CountryId);
            var city = _mapper.Map<IEnumerable<CityViewModel>>(await _cityClient.GetCitiesMaster());
            ViewBag.CityId = new SelectList(city, "Id", "Name", RestaurantBranch.CityId);
            ViewBag.Description = _userSessionManager.GetUserStore().Description;
            return View(RestaurantBranch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RestaurantBranchViewModel model)
        {
            try
            {

                model.RestaurantId = _userSessionManager.GetUserStore().Id;
                RestaurantBranchDTO Result = await _client.UpdateRestaurantBranchAsync(_mapper.Map<RestaurantBranchDTO>(model));


                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt") : "-",
                        Name = Result.NameAsPerTradeLicense,
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

            //}

            //return Json(new
            //{
            //    success = false,
            //    message = "Fill all required fields and submit the form again"
            //});

        }

        public async Task<IActionResult> ToggleCloseStatus(long id, TimeSpan? ClosingTimeSpan)
        {
            try
            {
                RestaurantBranchViewModel Result = _mapper.Map<RestaurantBranchViewModel>(await _client.ToggleCloseStatus(id, ClosingTimeSpan));

                return Json(new
                {
                    success = true,
                    message = string.Format("Branch {0} Successfully", Result.IsClose ? "Closed" : "Open"),
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, h:mm:ss"),
                        Name = Result.NameAsPerTradeLicense,
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

        public async Task<IActionResult> ToggleMainBranchStatus(long Id)
        {
            try
            {
                RestaurantBranchViewModel Result = _mapper.Map<RestaurantBranchViewModel>(await _client.ToggleMainStatus(Id));

                return Json(new
                {
                    success = true,
                    message = string.Format("Branch {0} Successfully", Result.IsClose ? "Closed" : "Open"),
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, h:mm:ss"),
                        Name = Result.NameAsPerTradeLicense,
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


        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                RestaurantBranchViewModel Result = _mapper.Map<RestaurantBranchViewModel>(await _client.ToggleActiveStatus(id));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate != null ? Result.CreationDate.ToString("dd MMM yyyy, h:mm:ss") : "-",
                        Name = Result.NameAsPerTradeLicense,
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
                await _client.DeleteRestaurantBranchAsync(id);

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

        //    public async Task<IActionResult> Details(long id)
        //    {
        //        return View(_mapper.Map<RestaurantBranchViewModel>(await _client.GetRestaurantBranchByIdAsync(id)));
        //    }

        //    public async Task<IActionResult> RestaurantBranchReport()
        //    {
        //        long restaurantId = _userSessionManager.GetUserStore().Id;
        //        var Info = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _client.GetAllRestaurantBranchsAsync(restaurantId));
        //        return new CSVResult<RestaurantBranchViewModel>(Info, "RestaurantBranch");
        //    }
    }

}
