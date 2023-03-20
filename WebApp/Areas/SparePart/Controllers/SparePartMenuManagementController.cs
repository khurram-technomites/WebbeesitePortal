using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]

    public class SparePartMenuManagementController : Controller
    {
        private readonly ISparePartMenuManagementClient _client;
        private readonly ISparePartMenuClient _menuClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;

        public SparePartMenuManagementController(ISparePartMenuManagementClient client, ISparePartMenuClient menuClient, IMapper mapper, IUserSessionManager userSessionManager)
        {
            _client = client;
            _menuClient = menuClient;
            _mapper = mapper;
            _userSessionManager = userSessionManager;

        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            IEnumerable<SparePartMenuManagementViewModel> model = _mapper.Map<IEnumerable<SparePartMenuManagementViewModel>>(await _client.GetAllBySparePartDealerIdAsync(SparePartId));
            return View(model.OrderBy(x => x.Position));
        }

        public async Task<IActionResult> AddSparePartMenu()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            var menu = await _menuClient.GetMenuBySparePartId(SparePartId);
            ViewBag.SparePartMenu = new SelectList(menu, "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSparePartMenu(long id)
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;

            if (id == 0)
            {
                return Json(new
                {
                    success = false,

                });
            }
            else
            {
                IEnumerable<SparePartMenuViewModel> SparePartMenus = _mapper.Map<IEnumerable<SparePartMenuViewModel>>(await _menuClient.GetAllByIdAsync(id));
                SparePartMenuViewModel SparePartMenu = SparePartMenus.FirstOrDefault();
                SparePartMenuManagementViewModel model = new SparePartMenuManagementViewModel();
                model.SparePartDealerId = SparePartId;
                model.SparePartMenuId = SparePartMenu.Id;
                model.Position = 0;

                var result = await _client.AddSparePartMenuManagementAsync(_mapper.Map<SparePartMenuManagementDTO>(model));


                return Json(new
                {

                    success = true,
                    data = new
                    {

                        Id = result.Id,
                        Title = SparePartMenu.Title,
                        Route = SparePartMenu.Route

                    }
                });
            }

        }

        public async Task<IActionResult> DeleteSparePartMenu(long Id)
        {
            await _client.DeleteSparePartMenuManagementAsync(Id);

            return Json(new
            {
                success = true,
                message = "SparePart Menu Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<SparePartMenuManagementDTO> positions)
        {
            try
            {
                SparePartMenuManagementViewModel model = new SparePartMenuManagementViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _client.SavePositions(_mapper.Map<SparePartMenuManagementDTO>(model));
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
