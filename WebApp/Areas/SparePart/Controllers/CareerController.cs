using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
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
    [Authorize("SparePartDealer")]
    public class CareerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ISparePartCareerClient _careersClient;

        public CareerController(IMapper mapper, IUserSessionManager userSessionManager, ISparePartCareerClient SparePartCareers)
        {
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _careersClient = SparePartCareers;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            IEnumerable<SparePartCareerViewModel> result = _mapper.Map<IEnumerable<SparePartCareerViewModel>>(await _careersClient.GetAllBySparePartDealerIdAsync(SparePartId));
            return View(result);
        }

        public async Task<IActionResult> MakeReport()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            IEnumerable<SparePartCareerViewModel> result = _mapper.Map<IEnumerable<SparePartCareerViewModel>>(await _careersClient.GetAllBySparePartDealerIdAsync(SparePartId));
            return new CSVResult<SparePartCareerViewModel>(result, "Careers");
        }
    }
}
