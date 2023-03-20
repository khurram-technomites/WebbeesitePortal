using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Restaurant Manager , Supplier")]
    public class RestaurantController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantClient _restaurantClient;
        private readonly IResturantClient _resturantClient;
        private readonly IRestaurantDocumentClient _restaurantDocumentClient;
        private readonly IRestaurantBranchScheduleClient _branchSchedule;
        private readonly IRestaurantBranchClient _restaurantBranchClient;
        private readonly ICountryClient _countryClient;
        readonly ICityClient _cityClient;
        private readonly IConfiguration _config;

        public RestaurantController(IMapper mapper, IRestaurantClient restaurantClient, IRestaurantBranchScheduleClient branchSchedule, IRestaurantBranchClient restaurantBranchClient,
            ICountryClient countryClient, ICityClient cityClient, IConfiguration config, IRestaurantDocumentClient restaurantDocumentClient, IResturantClient resturantClient)
        {
            _mapper = mapper;
            _restaurantClient = restaurantClient;
            _branchSchedule = branchSchedule;
            _restaurantBranchClient = restaurantBranchClient;
            _countryClient = countryClient;
            _cityClient = cityClient;
            _config = config;
            _restaurantDocumentClient = restaurantDocumentClient;
            _resturantClient = resturantClient;
        }

        public async Task<IActionResult> Index()
        {
            var restaurants = _mapper.Map<IEnumerable<RestaurantViewModel>>(await _restaurantClient.GetAll());
            return View(restaurants);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RestaurantViewModel model)
        {
            if (!string.IsNullOrEmpty(model.NameAsPerTradeLicense) && !string.IsNullOrEmpty(model.User.Email) && !string.IsNullOrEmpty(model.User.Password))
            {
                if (model.User.Email == "admin@fougito.com")
                    return Json(new { success = false, message = "Email cannot be admin@fougito.com" });

                model.PhoneNumber = "";
                model.Status = Enum.GetName(typeof(Status), Status.Active);
                model.Logo = _config.GetValue<string>("RestaurantDefaultLogoPath");
                model.Status = Enum.GetName(typeof(Status), Status.Inactive);
                RestaurantViewModel Result = _mapper.Map<RestaurantViewModel>(await _restaurantClient.Create(_mapper.Map<RestaurantDTO>(model)));
                RestaurantBranchDTO restaurantBranchDTO = new()
                {
                    Email = model.User.Email,
                    RestaurantId = Result.Id,
                    IsMainBranch = true,
                    NameAsPerTradeLicense = model.NameAsPerTradeLicense,
                    Status = Enum.GetName(typeof(Status), Status.Active)
                };
                await _restaurantBranchClient.AddRestaurantBranchAsync(restaurantBranchDTO);
                return Json(new
                {
                    success = true,
                    url = "/Admin/Restaurant/Index",
                    message = "Restaurant Created Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Logo + "|" + Result.NameArAsPerTradeLicense + "|" + Result.Email,
                        Status = Result.Status
                    }
                });
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var country = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryClient.GetCountriesByMaster());

            RestaurantViewModel Result = _mapper.Map<RestaurantViewModel>(await _restaurantClient.GetById(Id));
            Result.RestaurantBranches = Result.RestaurantBranches.Where(i => i.IsMainBranch == true).ToList();
            ViewBag.CountryId = new SelectList(country, "Id", "Name", Result.RestaurantBranches.FirstOrDefault().CountryId);
            return View(Result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RestaurantViewModel model)
        {
            model.RestaurantBranches[0].IsMainBranch = true;
            model.Origin = model.Origin.Replace("www.", "");
            RestaurantViewModel Result = _mapper.Map<RestaurantViewModel>(await _restaurantClient.Edit(_mapper.Map<RestaurantDTO>(model)));

            Result.Id = model.Id;

            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
            });
        }
        public IActionResult Schedule(long Id)
        {
            ViewBag.BranchId = Id;
            return View();
        }
        public async Task<IActionResult> GetSchedule(long Id)
        {
            var schedule = await _branchSchedule.GetAllRestaurantBranchSchedulesAsync(Id);
            return Json(new
            {
                success = true,
                data = schedule,
                message = "Success"
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
                message = "Record already exists!"
            });
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
        public async Task<IActionResult> Delete(long Id)
        {
            await _restaurantClient.Delete(Id);

            return Json(new
            {
                success = true,
                message = "Restaurant Deleted Successfully"
            });
        }
        public async Task<IActionResult> GetCityBycountryId(long countryId)
        {
            var cities = await _cityClient.GetCityByCountryId(countryId);

            return Json(new
            {
                success = true,
                data = cities
            });
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                RestaurantViewModel Result = _mapper.Map<RestaurantViewModel>(await _restaurantClient.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Logo + "|" + Result.NameAsPerTradeLicense + "|" + Result.Email,
                        Status = Result.Status
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
        public async Task<IActionResult> DeleteImage(long Id)
        {
            await _restaurantClient.DeleteImage(Id);

            return Json(new
            {
                success = true,
                message = "Schedule Deleted Successfully"
            });
        }

        public IActionResult DocumentModel(long RestaurantId)
        {
            return View(RestaurantId);
        }

        public async Task<IActionResult> DeleteDocument(long Id)
        {
            await _restaurantDocumentClient.DeleteDocument(Id);
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ..."
            });
        }

        public async Task<IActionResult> GetImages(long RestaurantId)
        {
            IEnumerable<RestaurantImagesViewModel> Images = _mapper.Map<IEnumerable<RestaurantImagesViewModel>>(await _restaurantClient.GetRestaurantImagesAsync(RestaurantId));
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ...",
                data = Images
            });
        }
        [HttpPost]
        public async Task<ActionResult> UpdateSecondaryLogo(long Id, string filePath)
        {
            try
            {

                RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturantClient.GetResturantByID(Id));
                model.Id = Id;
                model.SecondaryLogo = filePath;

                RestaurantDTO result = await _restaurantClient.Edit(_mapper.Map<RestaurantDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Logo updated successfully!",
                    filepath = result.Logo
                });

            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "OOops! Something went wrong. Please try later."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateThumbnailImage(long Id, string filePath)
        {
            try
            {

                RestaurantViewModel model = _mapper.Map<RestaurantViewModel>(await _resturantClient.GetResturantByID(Id));
                model.Id = Id;
                model.ThumbnailImage = filePath;

                RestaurantDTO result = await _resturantClient.Edit(_mapper.Map<RestaurantDTO>(model));
                return Json(new
                {
                    success = true,
                    message = "Logo updated successfully!",
                    filepath = result.Logo
                });

            }
            catch (ApiException ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Ops! Something went wrong. Please try later."
                });
            }
        }
    }
}