using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.Classes;
using WebApp.Services.TypedClients;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly ICityClient _cityService;
        private readonly ICountryClient _countryService;
        private readonly IMapper _mapper;
        [BindProperty]
        public CityViewModel Model { get; set; }
        public CityController(ICityClient cityService, ICountryClient countryService, IMapper mapper)
        {
            _cityService = cityService;
            _countryService = countryService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<CityViewModel>>(await _cityService.GetCities());
            return View(info);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            CityViewModel a = _mapper.Map<CityViewModel>(await _cityService.GetCityByID(Id));
            return View(a);
        }
        public async Task<ActionResult> Create()
        {
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CityViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    CityDTO Result = await _cityService.Create(_mapper.Map<CityDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/City/Index",
                        message = "City Created Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CountryName = Result.Name,
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                            CountryId = Result.CountryId
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/City/Index",
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
        public async Task<IActionResult> Edit(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    CityDTO Result = await _cityService.Edit(_mapper.Map<CityDTO>(model));

                    Result.Id = model.Id;
                    //if (Result != null)
                    //{
                    //    Result.Country = await _countryService.GetCountryByID(Result.CountryId);
                    //    Result.Id = model.Id;
                    //}

                    return Json(new
                    {
                        success = true,
                        message = "Record Updated Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CountryName = Result.Name,
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                            CountryId = Result.CountryId
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
        public async Task<IActionResult> Edit(long Id)
        {
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            CityViewModel cityDTO = _mapper.Map<CityViewModel>(await _cityService.GetCityByID(Id));
            return View(cityDTO);
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _cityService.Delete(Id);

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
        public async Task<IActionResult> CityMakeReport()
        {
            var info = _mapper.Map<List<CityViewModel>>(await _cityService.GetCities());
            return new CSVResult<CityViewModel>(info, "City");
        }
        public async Task<ActionResult> ToggleActiveStatus(long CityId)
        {
            try
            {
                CityViewModel Result = _mapper.Map<CityViewModel>(await _cityService.ToggleActiveStatus(CityId));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        CountryName = Result.Country.Name,
                        Name = Result.Name,
                        ID = Result.Id,
                        IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        CountryId = Result.CountryId
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
