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
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantProfileController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantClient _restaurantClient;
        private readonly IRestaurantDocumentClient _restaurantDocumentClient;
        private readonly IRestaurantBranchScheduleClient _branchSchedule;
        private readonly IRestaurantBranchClient _restaurantBranchClient;
        private readonly ICountryClient _countryClient;
        private readonly IUserSessionManager _userSession;
        readonly ICityClient _cityClient;
        private readonly IConfiguration _config;

        public RestaurantProfileController(IMapper mapper, IRestaurantClient restaurantClient, IRestaurantBranchScheduleClient branchSchedule, IRestaurantBranchClient restaurantBranchClient,
            ICountryClient countryClient, ICityClient cityClient, IConfiguration config, IRestaurantDocumentClient restaurantDocumentClient , IUserSessionManager userSession)
        {
            _mapper = mapper;
            _restaurantClient = restaurantClient;
            _branchSchedule = branchSchedule;
            _restaurantBranchClient = restaurantBranchClient;
            _countryClient = countryClient;
            _cityClient = cityClient;
            _config = config;
            _restaurantDocumentClient = restaurantDocumentClient;
            _userSession = userSession;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            long Id = _userSession.GetUserStore().Id;
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
            RestaurantViewModel Result = _mapper.Map<RestaurantViewModel>(await _restaurantClient.Edit(_mapper.Map<RestaurantDTO>(model)));

            Result.Id = model.Id;

            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
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
        public async Task<IActionResult> GetSchedule(long Id)
        {
            var schedule = await _branchSchedule.GetAllRestaurantBranchSchedulesAsync(Id);

            //if (schedule.Any())
            //{
            return Json(new
            {
                success = true,
                data = schedule,
                message = "Success"
            });
            //}

            //return Json(new
            //{
            //    success = false,
            //    message = "No shchedule found.. !"
            //});
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
    }
}
