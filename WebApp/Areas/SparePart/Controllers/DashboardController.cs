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
using WebApp.ViewModels.SparePart;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISparePartDashboardClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ISparePartCustomerAppointmentClient _customerAppointmentClient;
        [BindProperty]
        public SparePartDashboardViewModel Model { get; set; }
        public DashboardController(IMapper mapper, IUserSessionManager userSessionManager, ISparePartDashboardClient client,
            ISupplierOrderClient orderClient, ISparePartCustomerAppointmentClient customerAppointmentClient)
        {
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _client = client;
            _customerAppointmentClient = customerAppointmentClient;
        }
        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSessionManager.GetSparePartDealerStore().Id;
            var count = _mapper.Map<SparePartDashboardViewModel>(await _client.GetSparePartDashboardCount(SparePartId));
            count.CustomerAppointment = _mapper.Map<List<SparePartCustomerAppointmentViewModel>>(await _customerAppointmentClient.GetAllBySparePartDealerIdAsync(SparePartId));

            return View(count);
        }
    }
}
