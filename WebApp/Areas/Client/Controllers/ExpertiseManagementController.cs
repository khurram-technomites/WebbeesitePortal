using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    public class ExpertiseManagementController : Controller
    {
        private readonly IGarageExpertiseManagementClient _client;
        private readonly IGarageExpertiseClient _garageExpertiseClient;
        private readonly IExpertiseClient _expertiseClient;
        private readonly IMapper _mapper;
        private readonly IClientModulesClient _clientModule;
        private readonly IUserSessionManager _userSessionManager;

        public ExpertiseManagementController(
              IGarageExpertiseManagementClient client
            , IGarageExpertiseClient garageExpertiseClient
            , IExpertiseClient expertiseClient
            , IMapper mapper
            , IUserSessionManager userSessionManager
            , IClientModulesClient clientModule)
        {
            _client = client;
            _garageExpertiseClient = garageExpertiseClient;
            _expertiseClient = expertiseClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _clientModule = clientModule;

        }
        public async Task<IActionResult> Index()
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            var expertise = _mapper.Map<IEnumerable<GarageExpertiseManagementViewModel>>(await _client.GetAllByGarageIdAsync(garageId));
            return View(expertise);
        }

        public async Task<IActionResult> Details(long Id)
        {
            IEnumerable<GarageExpertiseManagementViewModel> garageManagements = _mapper.Map<IEnumerable<GarageExpertiseManagementViewModel>>(await _client.GetAllByIdAsync(Id));
            return View(garageManagements.FirstOrDefault());
        }
        public ActionResult Create()
        {
            GarageExpertiseManagementViewModel expertise = new GarageExpertiseManagementViewModel();
            return View(expertise);
        }

        [HttpPost]
        public async Task<ActionResult> Create(GarageExpertiseManagementViewModel model)
        {

            model.CreationDate = DateTime.UtcNow;
            string message = string.Empty;
            long garageId = _userSessionManager.GetGarageStore().Id;
            model.GarageId = garageId;
            if (ModelState.IsValid)
            {
                try
                {
                    var GarageExpertiseCount = await _client.GetCountAllByGarageIdAsync(garageId);
                    GarageExpertiseCount++;
                    var ClientModules = await _clientModule.GetClientModuleByClientId(garageId);
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Expertis && s.ClientId == garageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 || GarageExpertiseCount <= module)
                        {
                            GarageExpertiseManagementDTO Result = await _client.AddGarageExpertiseManagementAsync(_mapper.Map<GarageExpertiseManagementDTO>(model));

                            return Json(new
                            {
                                success = true,
                                url = "/Client/ExpertiseManagement/Index",
                                message = "Expertise Created Successfully",
                                data = new
                                {
                                    Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                                    Description = Result.Description,
                                    Id = Result.Id
                                }
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/ExpertiseManagement/Index",
                                message = $"You can't add more than {module} Expertise",
                            });
                        }
                    }
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/ExpertiseManagement/Index",
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
        public async Task<IActionResult> Edit(GarageExpertiseManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    long garageId = _userSessionManager.GetGarageStore().Id;
                    model.GarageId = garageId;
                    GarageExpertiseManagementDTO Result = await _client.UpdateGarageExpertiseManagementAsync(_mapper.Map<GarageExpertiseManagementDTO>(model));

                    Result.Id = model.Id;

                    return Json(new
                    {
                        success = true,
                        url = "/Garage/GarageExpertiseManagement/Index",
                        message = "Expertise Created Successfully",
                        data = new
                        {
                            Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Description = Result.Description,
                            Id = Result.Id
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
            IEnumerable<GarageExpertiseManagementViewModel> expertise = _mapper.Map<IEnumerable<GarageExpertiseManagementViewModel>>(await _client.GetAllByIdAsync(Id));
            var parent = _mapper.Map<IEnumerable<ExpertiseViewModel>>(await _expertiseClient.GetAllAsync());
            ViewBag.ExpertiseId = new SelectList(parent, "Id", "Title");
            return View(expertise.FirstOrDefault());
        }

        public async Task<IActionResult> Delete(long id)
        {
            var getExpertiseManagement = await _garageExpertiseClient.GetAllByGarageExpertiseManagementIdAsync(id);
            if (getExpertiseManagement.Any())
            {
                foreach (var item in getExpertiseManagement)
                {
                    await _garageExpertiseClient.DeleteGarageExpertiseAsync(item.Id);
                }
            }

            await _client.DeleteGarageExpertiseManagementAsync(id);

            return Json(new
            {

                success = true,
                message = "Expertise Management Deleted Successfully!"

            });

        }

        [HttpPost]
        public async Task<IActionResult> AddExpertise(long id, long managementId)
        {
            GarageExpertiseViewModel model = new GarageExpertiseViewModel();
            model.ExpertiseId = id;
            model.GarageExpertiseManagementId = managementId;

            var result = await _garageExpertiseClient.AddGarageExpertiseAsync(_mapper.Map<GarageExpertiseDTO>(model));

            var resultSets = await _expertiseClient.GetAllByIdAsync(result.ExpertiseId);
            var resultSet = resultSets.FirstOrDefault();


            return Json(new
            {

                success = true,
                data = new
                {
                    Title = resultSet.Title,
                    Id = result.Id,
                }

            });
        }

        public async Task<IActionResult> DeleteExpertise(long id)
        {
            await _garageExpertiseClient.DeleteGarageExpertiseAsync(id);

            return Json(new
            {

                success = true,
                message = "Expertise Deleted Successfully!"

            });

        }

        public async Task<IActionResult> GettAllExpertise(long id)
        {
            IEnumerable<GarageExpertiseViewModel> model = _mapper.Map<IEnumerable<GarageExpertiseViewModel>>(await _garageExpertiseClient.GetAllByGarageExpertiseManagementIdAsync(id));

            return Json(new
            {
                success = true,
                data = model.Select(i => new
                {

                    id = i.Id,
                    title = i.Expertise.Title,
                    i.ExpertiseId

                })

            });
        }




    }
}


