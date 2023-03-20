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
    [Authorize(Roles = "SparePartDealer")]
    public class SubscriberController : Controller
    {
        private readonly ISparePartSubscriberClient _subService;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;

        public SubscriberController(ISparePartSubscriberClient subService, IMapper mapper, IUserSessionManager userSession)
        {
            _subService = subService;
            _mapper = mapper;
            _userSession = userSession;
        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            var info = _mapper.Map<IEnumerable<SparePartSubscriberViewModel>>(await _subService.GetAllBySparePartDealerIdAsync(SparePartId));
            return View(info);
        }
        public async Task<IActionResult> MakeReport()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            var info = _mapper.Map<IEnumerable<SparePartSubscriberViewModel>>(await _subService.GetAllBySparePartDealerIdAsync(SparePartId));
            return new CSVResult<SparePartSubscriberViewModel>(info, "Subscribers");
        }
    }
}
