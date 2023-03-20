using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SparePartPartnersManagementController : Controller
    {
        private readonly ISparePartsDealerClient _SparePartClient;
        private readonly ISparePartPartnersManagementClient _PartnersManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        public SparePartPartnersManagementController(ISparePartsDealerClient SparePartClient, ISparePartPartnersManagementClient partnersManagementClient, IUserSessionManager userSession, IMapper mapper)
        {
            _SparePartClient = SparePartClient;
            _PartnersManagementClient = partnersManagementClient;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            IEnumerable<SparePartPartnersManagementViewModel> model = _mapper.Map<IEnumerable<SparePartPartnersManagementViewModel>>(await _PartnersManagementClient.GetAllBySparePartDealerIdAsync(SparePartId));

            return View(model.OrderBy(x => x.Position));


        }
        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartner(SparePartPartnersManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            Model.SparePartDealerId = SparePartId;
            var result = await _PartnersManagementClient.AddSparePartPartnersManagementAsync(_mapper.Map<SparePartPartnersManagementDTO>(Model));


            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartPartnersManagement/Index",
                message = "Record Added Successfully",
                data = new
                {
                    id = result.Id,
                    title = result.Title,
                    position = result.Position,
                    image = result.ImagePath
                }
            });
        }

        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SparePartPartnersManagementViewModel> result = _mapper.Map<IEnumerable<SparePartPartnersManagementViewModel>>(await _PartnersManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartPartnersManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartPartnersManagementDTO result = await _PartnersManagementClient.UpdateSparePartPartnersManagementAsync(_mapper.Map<SparePartPartnersManagementDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartPartnersManagement/Index",
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

        public async Task<IActionResult> DeleteSparePartPartner(long Id)
        {
            await _PartnersManagementClient.DeleteSparePartPartnersManagementAsync(Id);

            return Json(new
            {
                success = true,
                message = "SparePart Partner Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<SparePartPartnersManagementDTO> positions)
        {
            try
            {
                SparePartPartnersManagementViewModel model = new SparePartPartnersManagementViewModel();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _PartnersManagementClient.SavePositions(_mapper.Map<SparePartPartnersManagementDTO>(model));
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
