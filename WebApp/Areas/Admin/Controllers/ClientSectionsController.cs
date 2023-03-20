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
    public class ClientSectionsController : Controller
    {
        private readonly IClientSectionsClient _cityService;
        private readonly IMapper _mapper;
        [BindProperty]
        public ClientSectionsViewModel Model { get; set; }
        public ClientSectionsController(IClientSectionsClient cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<ClientSectionsViewModel>>(await _cityService.GetCities());
            return View(info);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<IActionResult> Details(long Id)
        {
            ClientSectionsViewModel a = _mapper.Map<ClientSectionsViewModel>(await _cityService.GetCityByID(Id));
            return View(a);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ClientSectionsViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Status = Enum.GetName(typeof(Status), Status.Active);
                    ClientSectionsDTO Result = await _cityService.Create(_mapper.Map<ClientSectionsDTO>(model));

                    return Json(new
                    {
                        success = true,
                        url = "/Admin/ClientSections/Index",
                        message = "Client Section Created Successfully",
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
                        url = "/Admin/ClientSections/Index",
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
        public async Task<IActionResult> Edit(ClientSectionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    ClientSectionsDTO Result = await _cityService.Edit(_mapper.Map<ClientSectionsDTO>(model));

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
        public async Task<IActionResult> Edit(long Id)
        {
            ClientSectionsViewModel cityDTO = _mapper.Map<ClientSectionsViewModel>(await _cityService.GetCityByID(Id));
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
            var info = _mapper.Map<List<ClientSectionsViewModel>>(await _cityService.GetCities());
            return new CSVResult<ClientSectionsViewModel>(info, "ClientSection");
        }
        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                ClientSectionsViewModel Result = _mapper.Map<ClientSectionsViewModel>(await _cityService.ToggleActiveStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
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
    }
}
