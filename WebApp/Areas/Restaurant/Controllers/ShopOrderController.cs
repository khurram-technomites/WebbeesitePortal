using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class ShopOrderController : Controller
    {
        private readonly ISupplierOrderClient _client;
        private readonly ISupplierClient _supplierClient;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;

        public ShopOrderController(ISupplierOrderClient client, ISupplierClient supplierClient, IUserSessionManager userSessionManager
          , IMapper mapper)
        {
            _client = client;
            _supplierClient = supplierClient;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
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
        [HttpPost]
        public async Task<IActionResult> FilterStatus(string status)
        {
            long restaurantId = _userSessionManager.GetUserStore().Id;
            SupplierOrderViewModel viewModel = new SupplierOrderViewModel();
            viewModel.RestaurantId = restaurantId;
            viewModel.Status = status;
            IEnumerable<SupplierOrderViewModel> model = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetAllRestaurantOrdersByStatus(_mapper.Map<SupplierOrderDTO>(viewModel)));
            return PartialView(model);
        }
        public async Task<IActionResult> ChangeStatus(long id)
        {
            IEnumerable<SupplierOrderViewModel> model = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetOrderByIdAsync(id));
            return View(model.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(long OrderId, string Status)
        {
            SupplierOrderViewModel viewModel = new SupplierOrderViewModel();
            viewModel.Id = OrderId;
            viewModel.Status = Status;

            SupplierOrderViewModel model = _mapper.Map<SupplierOrderViewModel>(await _client.UpdateStatusOrderAsync(_mapper.Map<SupplierOrderDTO>(viewModel)));

            IEnumerable<SupplierViewModel> suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierClient.GetSupplierById(model.SupplierId));
            var supplier = suppliers.FirstOrDefault();

            return Json(new
            {

                success = true,
                message = "Status updated successfully...!",
                data = new
                {
                    Date = model.CreationDate.ToString("dd MMM yyyy, h:mm tt"),
                    SupplierName = supplier != null ? supplier.NameAsPerTradeLicense : "-",
                    SupplierPhoneNumber = supplier != null ? supplier.PhoneNumber : "-",
                    OrderNo = model.OrderNo,
                    Total = model.Amount + model.Currency,
                    OrderDate =  model.OrderRequiredDate.ToString("dd MMM yyyy,h:mm:ss") ,
                    Status = model.Status,
                    Id = model.Id

                }

            });
        }
    }
}
