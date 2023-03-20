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
    public class CustomerAppointmentController : Controller
    {
        private readonly ISparePartCustomerAppointmentClient _CustomerAppointmentClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public CustomerAppointmentController(ISparePartCustomerAppointmentClient customerAppointmentClient, IMapper mapper, IUserSessionManager userSession)
        {
            _CustomerAppointmentClient = customerAppointmentClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            return View(_mapper.Map<IEnumerable<SparePartCustomerAppointmentViewModel>>(await _CustomerAppointmentClient.GetAllBySparePartDealerIdAsync(SparePartId)));
        }
        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<SparePartCustomerAppointmentViewModel> result = _mapper.Map<IEnumerable<SparePartCustomerAppointmentViewModel>>(await _CustomerAppointmentClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
    }
}
