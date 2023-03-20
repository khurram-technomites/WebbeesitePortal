using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using System.Collections.Generic;
using AutoMapper;

namespace WebApp.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class DashboardController : Controller
    {
        private readonly IDashboardClient _Service;
        private readonly IGarageClient _garageClient;
        private readonly ISparePartsDealerClient _sparePartsDealerClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _sessionManager;

        [BindProperty]
        public GarageDashboardViewModel Model { get; set; }
        public DashboardController(IMapper mapper, IUserSessionManager userSessionManager,
        ISupplierOrderClient orderClient, IDashboardClient service, IGarageClient garageClient, ISparePartsDealerClient sparePartsDealerClient, IUserSessionManager sessionManager)
        {
            _mapper = mapper;
            _sessionManager = sessionManager;
            _Service = service;
            _garageClient = garageClient;
            _sparePartsDealerClient = sparePartsDealerClient;
        }
        public async Task<IActionResult> Index()
        {
            long VendorId = _sessionManager.GetVendorStore().Id;
            var count = _mapper.Map<DashboardViewModel>(await _Service.GetVendorDashboardCount(VendorId));
            count.Garages = _mapper.Map<List<GarageViewModel>>(await _garageClient.GetGarageByVendor(VendorId));
            //count.SparePartsDealers = _mapper.Map<List<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());
            return View(count);
        }
        public async Task<IActionResult> List()
        {
            return View();
        }
    }
}
