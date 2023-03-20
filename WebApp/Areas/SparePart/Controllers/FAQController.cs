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
    public class FAQController : Controller
    {
        private readonly ISparePartsDealerClient _SparePartClient;
        private readonly ISparePartFAQClient _fAQClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        public FAQController(ISparePartsDealerClient SparePartClient, IUserSessionManager userSession, 
            IMapper mapper, ISparePartFAQClient fAQClient)
        {
            _SparePartClient = SparePartClient;
            _userSession = userSession;
            _mapper = mapper;
            _fAQClient = fAQClient;
        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            IEnumerable<SparePartFAQViewModel> model = _mapper.Map<IEnumerable<SparePartFAQViewModel>>(await _fAQClient.GetFAQBySparePartIdAsync(SparePartId));

            return View(model.OrderBy(x => x.Position));
        }

        public IActionResult AddPartner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPartner(SparePartFAQViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            Model.SparePartId = SparePartId;
            var result = await _fAQClient.AddFAQAsync(_mapper.Map<SparePartFAQDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/FAQ/Index",
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
            return View(_mapper.Map<SparePartFAQViewModel>(await _fAQClient.GetFAQByIdAsync(Id)));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartFAQViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartId = SparePartId;
            SparePartFAQDTO result = await _fAQClient.UpdateFAQAsync(_mapper.Map<SparePartFAQDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/FAQ/Index",
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

        public async Task<IActionResult> DeleteSparePartPartner(long Id)
        {
            await _fAQClient.ArchiveFAQAsync(Id);

            return Json(new
            {
                success = true,
                message = "FAQ Deleted Successfully!"
            });
        }

        [HttpPost]
        public async Task<ActionResult> SavePosition(List<SparePartFAQViewModel> positions)
        {
            try
            {
                SparePartFAQViewModel model = new();
                foreach (var item in positions)
                {
                    model.Position = item.Position;
                    model.Id = item.Id;
                    await _fAQClient.SavePosition(_mapper.Map<SparePartFAQDTO>(model));
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
