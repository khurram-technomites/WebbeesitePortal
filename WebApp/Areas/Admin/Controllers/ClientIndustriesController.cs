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
    public class ClientIndustriesController : Controller
    {
        private readonly IClientIndustriesClient _cityService;
        private readonly IMapper _mapper;
        [BindProperty]
        public ClientIndustriesViewModel Model { get; set; }
        public ClientIndustriesController(IClientIndustriesClient cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<ClientIndustriesViewModel>>(await _cityService.GetIndustries());
            return View(info);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            ClientIndustriesViewModel a = _mapper.Map<ClientIndustriesViewModel>(await _cityService.GetCityByID(Id));
            return View(a);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ClientIndustriesViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = Enum.GetName(typeof(Status), Status.Active);
                    ClientIndustriesDTO Result = await _cityService.Create(_mapper.Map<ClientIndustriesDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/ClientIndustries/Index",
                        message = "Client Industries Created Successfully",
                        data = new
                        {
                            CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            CountryName = Result.Name,
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Admin/ClientIndustries/Index",
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
        public async Task<IActionResult> Edit(ClientIndustriesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    ClientIndustriesDTO Result = await _cityService.Edit(_mapper.Map<ClientIndustriesDTO>(model));

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
                            Name = Result.Name,
                            ID = Result.Id,
                            IsActive = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
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
            ClientIndustriesViewModel cityDTO = _mapper.Map<ClientIndustriesViewModel>(await _cityService.GetCityByID(Id));
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
            var info = _mapper.Map<List<ClientIndustriesViewModel>>(await _cityService.GetIndustries());
            return new CSVResult<ClientIndustriesViewModel>(info, "ClientIndustries");
        }
        public async Task<ActionResult> ToggleActiveStatus(long CityId)
        {
            try
            {
                ClientIndustriesViewModel Result = _mapper.Map<ClientIndustriesViewModel>(await _cityService.ToggleActiveStatus(CityId));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        CreationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,
                        ID = Result.Id,
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
    }
}
