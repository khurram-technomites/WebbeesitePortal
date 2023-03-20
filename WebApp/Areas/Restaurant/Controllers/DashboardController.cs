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

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]

    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantDashboardClient _service;
        private readonly IRestaurantClient _client;
        private readonly ICustomerClient _customer;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IRestaurantOrderClient _restaurantOrderClient;
        private readonly IRestaurantBranchClient _branchclient;
        [BindProperty]
        public RestaurantDashboardViewModel Model { get; set; }
        public DashboardController(IRestaurantDashboardClient service, IMapper mapper, ICustomerClient customer , IUserSessionManager userSessionManager,
            IRestaurantClient client , IRestaurantOrderClient restaurantOrderClient , IRestaurantBranchClient branchclient)
        {
            _mapper = mapper;
            _service = service;
            _customer = customer;
            _userSessionManager = userSessionManager;
            _client = client;
            _restaurantOrderClient = restaurantOrderClient;
            _branchclient = branchclient;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var count = _mapper.Map<RestaurantDashboardViewModel>(await _service.GetRestaurantDashboardCount(restaurantId));
            count.Orders = _mapper.Map<List<RestaurantOrderViewModel>>(await _restaurantOrderClient.GetAllOrderByRestaurantAsync(restaurantId));
            var parent = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _branchclient.GetAllRestaurantBranchsAsync(restaurantId));
            ViewBag.RestaurantBranch = new SelectList(parent, "Id", "NameAsPerTradeLicense");
            //count.CustomerCount = _mapper.Map<List<CustomerViewModel>>(await _customer.GetCustomers());
            //count.UserCount = _mapper.Map<List<UserViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());

            return View(count);
        }

        public async Task<IActionResult> OrderStats(long id = 0) 
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            DashboardStatsDTO statsDTO = _mapper.Map<DashboardStatsDTO>(await _service.GetPaymentMethodStats(restaurantId, id));
            return Json(new
            {
                success  = true,
                result = statsDTO
            });
        }
    }
}
