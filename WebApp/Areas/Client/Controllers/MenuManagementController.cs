using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using HelperClasses.DTOs;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    public class MenuManagementController : Controller
    {
        private readonly IGarageMenuManagementClient _client;
        private readonly IGarageMenuClient _menuClient;
        private readonly IGarageClient _garageClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public MenuManagementController(IGarageMenuManagementClient client ,
            IGarageClient garageClient, IGarageMenuClient menuClient , IMapper mapper , IUserSessionManager userSessionManager)
        {
            _client = client;
            _menuClient = menuClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _garageClient = garageClient;

        }

        public async Task<IActionResult> Index()
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            IEnumerable<GarageMenuManagementViewModel> model = _mapper.Map<IEnumerable<GarageMenuManagementViewModel>>(await _client.GetAllByGarageIdAsync(garageId));
            return View(model.OrderBy(x => x.Position));
        }

        public async Task<IActionResult> AddGarageMenu()
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            var menu = await _menuClient.GetMenuByGarageId(garageId);
            ViewBag.GarageMenu = new SelectList(menu, "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGarageMenu(long id)
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            GarageViewModel garageModel = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(garageId));
            IEnumerable<GarageMenuViewModel> garageMenus = _mapper.Map<IEnumerable<GarageMenuViewModel>>(await _menuClient.GetAllByIdAsync(id));
            GarageMenuViewModel garageMenu = garageMenus.FirstOrDefault();
            GarageMenuManagementViewModel model = new GarageMenuManagementViewModel();
            model.GarageId = garageId;
            model.GarageMenuId = garageMenu.Id;
            model.Position = 0;
            garageModel.GarageContentManagement = null;
            garageModel.User = null;
            if (garageMenu.Title == "Blogs")
            {
                //garageModel.Id = garageId;
                garageModel.IsBlogsAllowed = true;
                await _garageClient.UpdateGarage(_mapper.Map<GarageDTO>(garageModel));
            }
            else if (garageMenu.Title == "Services")
            {
                //garageModel.Id = garageId;
                garageModel.IsServicesAllowed = true;
                await _garageClient.UpdateGarage(_mapper.Map<GarageDTO>(garageModel));
            }
            else if (garageMenu.Title == "Appoinment")
            {
                //garageModel.Id = garageId;
                garageModel.IsAppoinmnetsAllowed = true;
                await _garageClient.UpdateGarage(_mapper.Map<GarageDTO>(garageModel));
            }

            var result = await _client.AddGarageMenuManagementAsync(_mapper.Map<GarageMenuManagementDTO>(model));

            
            return Json(new { 
            
                success = true,
                data = new { 
                
                    Id = result.Id,
                    Title = garageMenu.Title,
                    Route = garageMenu.Route
                
                }
            });
        }

        public async Task<IActionResult> DeleteGarageMenu(long Id , long garageMenuId)
        {
            long garageId = _userSessionManager.GetGarageStore().Id;
            GarageViewModel garageModel = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(garageId));
            IEnumerable<GarageMenuViewModel> garageMenus = _mapper.Map<IEnumerable<GarageMenuViewModel>>(await _menuClient.GetAllByIdAsync(garageMenuId));
            GarageMenuViewModel garageMenu = garageMenus.FirstOrDefault();
            GarageMenuManagementViewModel model = new GarageMenuManagementViewModel();
            garageModel.GarageContentManagement = null;
            garageModel.User = null;
            if (garageMenu.Title == "Blogs")
            {
                //garageModel.Id = garageId;
                garageModel.IsBlogsAllowed = false;
                await _garageClient.UpdateGarage(_mapper.Map<GarageDTO>(garageModel));
            }
            else if (garageMenu.Title == "Services")
            {
                //garageModel.Id = garageId;
                garageModel.IsServicesAllowed = false;
                await _garageClient.UpdateGarage(_mapper.Map<GarageDTO>(garageModel));
            }
            
            await _client.DeleteGarageMenuManagementAsync(Id);

            return Json(new
            {
                success = true,
                message = "Client Menu Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<GarageMenuManagementDTO> positions)
        {
            try
            {
                GarageMenuManagementViewModel model = new GarageMenuManagementViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _client.SavePositions(_mapper.Map<GarageMenuManagementDTO>(model));
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
