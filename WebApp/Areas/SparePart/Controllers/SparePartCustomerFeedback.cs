using AutoMapper;
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
    public class SparePartCustomerFeedbackController : Controller
    {
        private readonly ISparePartCustomerFeedbackClient _CustomerFeedbackClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public SparePartCustomerFeedbackController(ISparePartCustomerFeedbackClient customerFeedbackClient, IMapper mapper, IUserSessionManager userSession)
        {
            _CustomerFeedbackClient = customerFeedbackClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            return View(_mapper.Map<IEnumerable<SparePartCustomerFeedbackViewModel>>(await _CustomerFeedbackClient.GetAllBySparePartDealerIdAsync(SparePartId)));
        }
    }
}
