using AutoMapper;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class FAQController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IGarageFAQClient _fAQClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        public FAQController(IGarageClient garageClient, IUserSessionManager userSession, 
            IMapper mapper, IGarageFAQClient fAQClient)
        {
            _garageClient = garageClient;
            _userSession = userSession;
            _mapper = mapper;
            _fAQClient = fAQClient;
        }
        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            IEnumerable<GarageFAQViewModel> model = _mapper.Map<IEnumerable<GarageFAQViewModel>>(await _fAQClient.GetFAQByGarageAsync(GarageId));

            return View(model.OrderBy(x => x.Position));
        }

        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartner(GarageFAQViewModel Model)
        {
            long garageId = _userSession.GetGarageStore().Id;
            Model.GarageId = garageId;
            var result = await _fAQClient.AddFAQAsync(_mapper.Map<GarageFAQDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/Garage/FAQ/Index",
                message = "Record Added Successfully",
                data = new
                {
                    id = result.Id,
                    question = result.Question,
                    answer = result.Answer,
                    position = result.Position
                }
            });
        }

        public async Task<IActionResult> Edit(long Id)
        {
            return View(_mapper.Map<GarageFAQViewModel>(await _fAQClient.GetFAQByIdAsync(Id)));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageFAQViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            Model.GarageId = GarageId;
            GarageFAQDTO result = await _fAQClient.UpdateFAQAsync(_mapper.Map<GarageFAQDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/Garage/FAQ/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    id = result.Id,
                    question = result.Question,
                    answer = result.Answer,
                    position = result.Position
                }
            });
        }

        public async Task<IActionResult> DeleteGaragePartner(long Id)
        {
            await _fAQClient.ArchiveFAQAsync(Id);

            return Json(new
            {
                success = true,
                message = "FAQ Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<GarageFAQViewModel> positions)
        {
            try
            {
                GarageFAQViewModel model = new();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _fAQClient.SavePosition(_mapper.Map<GarageFAQDTO>(model));
                }

                return Json(new { success = true, message = "Position successfully updated..." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Something went wrong!" });
            }
        }
    }
}
