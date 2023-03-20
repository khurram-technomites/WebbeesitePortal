using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class ServiceManagementController : Controller
    {
        private readonly IGarageServiceManagementClient _serviceManagementClient;
        private readonly IGarageClient _garageClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        private readonly IClientModulesClient _clientModule;


        public ServiceManagementController(IGarageServiceManagementClient serviceManagementClient,
            IMapper mapper,
            IUserSessionManager userSession
            , IGarageClient garageClient
            , IClientModulesClient clientModule)
        {
            _serviceManagementClient = serviceManagementClient;
            _mapper = mapper;
            _userSession = userSession;
            _garageClient = garageClient;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageServiceManagementViewModel>>(await _serviceManagementClient.GetAllByGarageIdAsync(GarageId)));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageServiceManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            Model.Status = Enum.GetName(typeof(Status), Status.Active);
            var GarageServiceCount = await _serviceManagementClient.GetCountAllByGarageIdAsync(GarageId);
            GarageServiceCount++;
            var ClientModules = await _clientModule.GetClientModuleByClientId(GarageId);
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Services && s.ClientId == GarageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 || GarageServiceCount <= module)
                        {
                            GarageServiceManagementDTO Result = await _serviceManagementClient.AddGarageServiceManagementAsync(_mapper.Map<GarageServiceManagementDTO>(Model));
                            return Json(new
                            {
                                success = true,
                                message = "Service added successfully!",
                                data = new
                                {
                                    id = Result.Id,
                                    date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                                    icon = Result.Icon,
                                    title = Result.Title,
                                    status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                                }
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                message = $"You can't add more than {module} Services",
                            });
                        }
                    }
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
            IEnumerable<GarageServiceManagementViewModel> result = _mapper.Map<IEnumerable<GarageServiceManagementViewModel>>(await _serviceManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageServiceManagementViewModel Model)
        {
            //long GarageId = _userSession.GetGarageStore().Id;
            long GarageId = _userSession.GetGarageStore().Id;
            GarageViewModel garages = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(GarageId));
            GarageDTO garage = new();
            garage.IsServicesAllowed = garages.IsServicesAllowed;
            garage.IsBlogsAllowed = garages.IsBlogsAllowed;
            garage.IsCareersAllowed = garages.IsCareersAllowed;
            garage.IsFeedbackAllowed = garages.IsFeedbackAllowed;
            garage.IsAppoinmnetsAllowed = garages.IsAppoinmnetsAllowed;
            garage.IsTeamsAllowed = garages.IsTeamsAllowed;

            Model.GarageId = GarageId;
            GarageServiceManagementDTO result = await _serviceManagementClient.UpdateGarageServiceManagementAsync(_mapper.Map<GarageServiceManagementDTO>(Model));

            return Json(new
            {
                success = true,
                message = "Service Updated successfully!",
                data = new
                {

                    id = result.Id,
                    date = result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                    icon = result.Icon,
                    title = result.Title,
                    status = result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,



                }
            });
        }

        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<GarageServiceManagementViewModel> result = _mapper.Map<IEnumerable<GarageServiceManagementViewModel>>(await _serviceManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceManagementClient.DeleteGarageServiceManagementAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Service Deleted Successfully"
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

        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                long GarageId = _userSession.GetGarageStore().Id;
                GarageViewModel garages = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(GarageId));
                GarageDTO garage = new();
                garage.IsServicesAllowed = garages.IsServicesAllowed;
                garage.IsBlogsAllowed = garages.IsBlogsAllowed;
                garage.IsCareersAllowed = garages.IsCareersAllowed;
                garage.IsFeedbackAllowed = garages.IsFeedbackAllowed;
                garage.IsAppoinmnetsAllowed = garages.IsAppoinmnetsAllowed;
                garage.IsTeamsAllowed = garages.IsTeamsAllowed;
                GarageServiceManagementViewModel Result = _mapper.Map<GarageServiceManagementViewModel>(await _serviceManagementClient.ToggleStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                        Icon = Result.Icon + "|" + Result.Title,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
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
