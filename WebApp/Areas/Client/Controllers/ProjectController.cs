using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.Areas.Admin.Models;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class ProjectController : Controller
    {
        private readonly IGarageProjectClient _projectService;
        private readonly IMapper _mapper;
        private readonly IClientModulesClient _clientModule;
        private readonly IUserSessionManager _sessionManager;
        public ProjectController(IGarageProjectClient projectService, IMapper mapper, IUserSessionManager sessionManager,
            IClientModulesClient clientModule)
        {
            _projectService = projectService;
            _mapper = mapper;
            _sessionManager = sessionManager;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _sessionManager.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageProjectViewModel>>(await _projectService.GetByGarageAsync(GarageId)));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageProjectViewModel Model)
        {
            long GarageId = _sessionManager.GetGarageStore().Id;
            Model.GarageId = GarageId;
            if (ModelState.IsValid)
            {
                try
                {
                    var GarageProjectCount = await _projectService.GetCountByGarageAsync(GarageId);
                    GarageProjectCount++;
                    var ClientModules = await _clientModule.GetClientModuleByClientId(GarageId);
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Project && s.ClientId == GarageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 ||GarageProjectCount <= module)
                        {
                            GarageProjectDTO Result = await _projectService.AddProjectAsync(_mapper.Map<GarageProjectDTO>(Model));

                            return Json(new
                            {
                                success = true,
                                url = "/Client/Project/Index",
                                message = "Record Added Successfully",
                                data = new
                                {
                                    ID = Result.Id,
                                    Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                                    Title = Result.Title,
                                    Description = Result.Description,
                                }

                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/Project/Index",
                                message = $"You can't add more than {module} Projects",
                            });
                        }
                    }



                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/Project/Index",
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
            GarageProjectViewModel result = _mapper.Map<GarageProjectViewModel>(await _projectService.GetByIdAsync(Id));
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageProjectViewModel model)
        {

            try
            {
                GarageProjectDTO Result = await _projectService.UpdateProjectAsync(_mapper.Map<GarageProjectDTO>(model));

                Result.Id = model.Id;
                model.Status = Result.Status;
                return Json(new
                {
                    success = true,
                    url = "/Client/Project/Index",
                    message = "Record Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Title = Result.Title,
                        Description = Result.Description,
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



            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });

        }

        public async Task<IActionResult> Details(long Id)
        {
            GarageProjectViewModel result = _mapper.Map<GarageProjectViewModel>(await _projectService.GetByIdAsync(Id));
            return View(result);
        }

        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _projectService.ArchiveProjectAsync(Id);

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

        public async Task<ActionResult> ToggleActiveStatus(long id)
        {
            try
            {
                GarageProjectViewModel Result = _mapper.Map<GarageProjectViewModel>(await _projectService.ToggleStatusAsync(id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Title = Result.Title,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false
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
