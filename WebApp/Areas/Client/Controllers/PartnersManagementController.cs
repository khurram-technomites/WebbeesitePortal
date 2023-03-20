using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Services.TypedClients;
using WebApp.ViewModels;
using System.Linq;
using WebApp.Services;
using HelperClasses.DTOs.GarageCMS;
using DocumentFormat.OpenXml.Office2010.Excel;
using WebAPI.Models;
using Newtonsoft.Json;
using WebApp.ErrorHandling;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class PartnersManagementController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IGaragePartnersManagementClient _PartnersManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        private readonly IClientModulesClient _clientModule;
        public PartnersManagementController(IGarageClient garageClient,
            IGaragePartnersManagementClient partnersManagementClient,
            IUserSessionManager userSession,
            IMapper mapper
            , IClientModulesClient clientModule)
        {
            _garageClient = garageClient;
            _PartnersManagementClient = partnersManagementClient;
            _userSession = userSession;
            _mapper = mapper;
            _clientModule = clientModule;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            IEnumerable<GaragePartnersManagementViewModel> model = _mapper.Map<IEnumerable<GaragePartnersManagementViewModel>>(await _PartnersManagementClient.GetAllByGarageIdAsync(GarageId));

            return View(model.OrderBy(x => x.Position));
        }
        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartner(GaragePartnersManagementViewModel Model)
        {
            long garageId = _userSession.GetGarageStore().Id;
            Model.GarageId = garageId;
            if (ModelState.IsValid)
            {
                try
                {
                    var GaragePartnerCount = await _PartnersManagementClient.GetAllCountByGarageIdAsync(garageId);
                    GaragePartnerCount++;
                    var ClientModules = await _clientModule.GetClientModuleByClientId(garageId);
                    foreach (var ClientModule in ClientModules.Where(s => s.Module.ServiceName == ModulesObject.Blogs && s.ClientId == garageId))
                    {
                        long module = ClientModule.Quantity;
                        if (module == 0 || GaragePartnerCount <= module)
                        {
                            var Result = await _PartnersManagementClient.AddGaragePartnersManagementAsync(_mapper.Map<GaragePartnersManagementDTO>(Model));


                            return Json(new
                            {
                                success = true,
                                url = "/Client/PartnersManagement/Index",
                                message = "Record Added Successfully",
                                data = new
                                {
                                    id = Result.Id,
                                    title = Result.Title,
                                    position = Result.Position,
                                    image = Result.ImagePath
                                }
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                url = "/Client/Blog/Index",
                                message = $"You can't add more than {module} Blogs",
                            });
                        }
                    }



                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Client/Blog/Index",
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
            IEnumerable<GaragePartnersManagementViewModel> result = _mapper.Map<IEnumerable<GaragePartnersManagementViewModel>>(await _PartnersManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GaragePartnersManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            GaragePartnersManagementDTO result = await _PartnersManagementClient.UpdateGaragePartnersManagementAsync(_mapper.Map<GaragePartnersManagementDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/Garage/GaragePartnersManagement/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    id = result.Id,
                    title = result.Title,
                    position = result.Position,
                    image = result.ImagePath
                }
            });
        }

        public async Task<IActionResult> DeleteGaragePartner(long Id)
        {
            await _PartnersManagementClient.DeleteGaragePartnersManagementAsync(Id);

            return Json(new
            {
                success = true,
                message = "Garage Partner Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<GaragePartnersManagementDTO> positions)
        {
            try
            {
                GaragePartnersManagementViewModel model = new GaragePartnersManagementViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _PartnersManagementClient.SavePositions(_mapper.Map<GaragePartnersManagementDTO>(model));
                }

                return Json(new { success = true, message = "Position successfully updated..." });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }
        }
    }
}
