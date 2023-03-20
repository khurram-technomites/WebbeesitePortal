using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using WebApp.ViewModels;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Linq;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebAPI.Models;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class TeamManagementController : Controller
    {
        private readonly IGarageTeamManagementClient _serviceTeamManagementClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        private readonly IClientModulesClient _clientModule;
        public TeamManagementController(IGarageTeamManagementClient serviceTeamManagementClient,
            IMapper mapper, IUserSessionManager userSession
            , IClientModulesClient clientModule)
        {
            _serviceTeamManagementClient = serviceTeamManagementClient;
            _mapper = mapper;
            _userSession = userSession;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllByGarageIdAsync(GarageId)));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageTeamManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            if (ModelState.IsValid)
            {
                try
                {
                    var GarageTeamManagementCount = await _serviceTeamManagementClient.GetCountAllByGarageIdAsync(GarageId);
                    GarageTeamManagementCount++;
                    var ClientModules = await _clientModule.GetClientModuleByClientId(GarageId);
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Teams && s.ClientId == GarageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 ||GarageTeamManagementCount <= module)
                        {
                            GarageTeamManagementDTO result = await _serviceTeamManagementClient.AddGarageTeamManagementAsync(_mapper.Map<GarageTeamManagementDTO>(Model));

                            return Json(new
                            {
                                success = true,
                                url = "/Client/TeamManagement/Index",
                                message = "Record Added Successfully",
                                data = new
                                {
                                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                                    Name = result.Name,
                                    ImagePath = result.ImagePath,
                                    Designation = result.Designation,
                                    Id = result.Id,
                                }
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/TeamManagement/Index",
                                message = $"You can't add more than {module} Teams",
                            });
                        }
                    }



                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/TeamManagement/Index",
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
            IEnumerable<GarageTeamManagementViewModel> result = _mapper.Map<IEnumerable<GarageTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageTeamManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            GarageTeamManagementDTO result = await _serviceTeamManagementClient.UpdateGarageTeamManagementAsync(_mapper.Map<GarageTeamManagementDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/Garage/GarageTeamManagement/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.Name,
                    ImagePath = result.ImagePath,
                    Designation = result.Designation,
                    Id = result.Id,
                }
            });
        }

        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<GarageTeamManagementViewModel> result = _mapper.Map<IEnumerable<GarageTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceTeamManagementClient.DeleteGarageTeamManagementAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Team Deleted Successfully"
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
