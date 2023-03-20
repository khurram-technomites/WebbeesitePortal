using AutoMapper;
using HelperClasses.DTOs.Order;
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

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Supplier")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISupplierDashboardClient _client;
        private readonly ISupplierOrderClient _orderClient;
        private readonly IUserSessionManager _userSessionManager;
        [BindProperty]
        public SupplierDashboardViewModel Model { get; set; }
        public DashboardController( IMapper mapper, IUserSessionManager userSessionManager , ISupplierDashboardClient client,
            ISupplierOrderClient orderClient)
        {
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _client = client;
            _orderClient = orderClient;
        }
        public async Task<IActionResult> Index()
        {
            long supplierId = _userSessionManager.GetUserStore().Id;
            var count = _mapper.Map<SupplierDashboardViewModel>(await _client.GetSupplierDashboardCount(supplierId));
            count.Orders = _mapper.Map<List<SupplierOrderViewModel>>(await _orderClient.GetOrderBySupplierIdAsync(supplierId));

            return View(count);
        }
    }
}
