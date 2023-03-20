using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SupplierOrderController : Controller
    {
        private readonly ISupplierOrderClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ICustomerTransactionHistoryClient _trasactionClient;
        private readonly IMapper _mapper;
        public SupplierOrderController(ISupplierOrderClient client, IUserSessionManager userSessionManager
            , IMapper mapper, ICustomerTransactionHistoryClient trasactionClient)
        {
            _client = client;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
            _trasactionClient = trasactionClient;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetAllSupplierOrderByRestaurantAsync(restaurantId));
            return View(Info);
        }
        public async Task<IActionResult> Details(long id)
        {
            var model = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetOrderByIdAsync(id));
            return View(model.FirstOrDefault());
        }
    }
}
