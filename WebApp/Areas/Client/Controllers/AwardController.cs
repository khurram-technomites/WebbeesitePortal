using AutoMapper;
using HelperClasses.Classes;
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
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    //[Authorize(Roles = "Garage")]

    public class AwardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGarageAwardClient _client;
        private readonly IFileUpload _fileUpload;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IClientModulesClient _clientModule;


        public AwardController(IMapper mapper, IGarageAwardClient client, IFileUpload fileUpload,
            IUserSessionManager userSessionManager
            , IClientModulesClient clientModule)
        {
            _mapper = mapper;
            _fileUpload = fileUpload;
            _client = client;
            _userSessionManager = userSessionManager;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            var Info = _mapper.Map<IEnumerable<GarageAwardViewModel>>(await _client.GetAllGarageAwardsAsync(garageId));
            return View(Info);
        }

        public async Task<IActionResult> Create()
        {
            return View(new GarageAwardViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageAwardViewModel model)
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            model.GarageId = garageId;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    var GarageAwardCount = await _client.GetCountAllGarageAwardsAsync(garageId);
                    GarageAwardCount++;
                    var clientModules = await _clientModule.GetClientModuleByClientId(garageId);
                    foreach (var clientModule in clientModules.Where(s => s.Module.ServiceName == ModulesObject.Award && s.ClientId == garageId))
                    {
                        long module = clientModule.Quantity;
                        if (module == 0 || GarageAwardCount <= module)
                        {
                            GarageAwardDTO Result = await _client.AddGarageAwardAsync(_mapper.Map<GarageAwardDTO>(model));

                            return Json(new
                            {
                                success = true,
                                url = "/Client/Award/Index",
                                message = "Record Created Successfully!",
                                data = new
                                {
                                    ID = Result.Id,
                                    Date = Result.CreationDate.ToString("dd MMM yyyy , hh:mm tt"),
                                    Name = Result.Name,

                                }
                            });

                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/Award/Index",
                                message = $"You can't add more than {module} Awards",
                            });
                        }
                    }



                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/Award/Index",
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

        public async Task<IActionResult> Edit(long id)
        {
            GarageAwardViewModel GarageAwardDTO = _mapper.Map<GarageAwardViewModel>(await _client.GetGarageAwardByIdAsync(id));
            return View(GarageAwardDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageAwardViewModel model)
        {
            try
            {
                long garageId = _userSessionManager.GetGarageStore().Id;
                model.GarageId = garageId;
                GarageAwardDTO Result = await _client.UpdateGarageAwardAsync(_mapper.Map<GarageAwardDTO>(model));

                Result.Id = model.Id;

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully",
                    data = new
                    {
                        ID = Result.Id,
                        Date = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        Name = Result.Name,

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _client.DeleteGarageAwardAsync(id);

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

        public async Task<IActionResult> Details(long id)
        {
            GarageAwardViewModel model = _mapper.Map<GarageAwardViewModel>(await _client.GetGarageAwardByIdAsync(id));
            return View(model);
        }


    }

}
