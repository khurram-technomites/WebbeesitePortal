using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryClient _countryService;
        private readonly IMapper _mapper;
        [BindProperty]
        public CountryViewModel Model { get; set; }
        public CountryController(ICountryClient countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries()));
        }
        public async Task<ActionResult> List()
        {

            return PartialView();
        }
        public async Task<IActionResult> Details(long CountryId)
        {
            return View(_mapper.Map<CountryViewModel>(await _countryService.GetCountryByID(CountryId)));
        }
        public async Task<ActionResult> ToggleActiveStatus(long CountryId)
        {
            try
            {
                CountryViewModel Result = _mapper.Map<CountryViewModel>(await _countryService.ToggleActiveStatus(CountryId));

                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        ID = Result.Id
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
    }
}
