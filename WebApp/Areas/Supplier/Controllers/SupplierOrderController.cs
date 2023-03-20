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

namespace WebApp.Areas.Supplier.Controllers
{
    [Area("Supplier")]
    [Authorize(Roles = "Supplier")]
    public class SupplierOrderController : Controller
    {
        private readonly ISupplierOrderClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly ICustomerTransactionHistoryClient _trasactionClient;
        private readonly IMapper _mapper;
        string[] date;
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
            long supplierId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetOrderBySupplierIdAsync(supplierId));
            return View(Info);
        }
        public async Task<IActionResult> List()
        {
            long supplierId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetOrderBySupplierIdAsync(supplierId));
            return PartialView(Info);
        }
        public async Task<IActionResult> Details(long id)
        {
            //IEnumerable<CustomerTransactionHistoryViewModel> transactrion = _mapper.Map<IEnumerable<CustomerTransactionHistoryViewModel>>(await _trasactionClient.GetTransactionsByOrderIdAsync(id));
            var model = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetOrderByIdAsync(id));
            //model.Transaction = transactrion.FirstOrDefault();
            return View(model.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> SupplierList(string status , string orderRequiredDate)
        {
            SupplierOrderViewModel viewModel = new SupplierOrderViewModel();
            if (orderRequiredDate != null)
            {
                date = orderRequiredDate.Split(' ');
                string Date1 = date[0];
                string Date2 = date[2];
                viewModel.OrderRequiredDate = DateTime.Parse(Date1).Date ;
                viewModel.OrderRequiredDate2 = DateTime.Parse(Date2).Date ;
            }
            long supplierId = _userSessionManager.GetUserStore().Id;
            
 
            viewModel.SupplierId = supplierId;
            viewModel.Status = status;
            IEnumerable<SupplierOrderViewModel> model = _mapper.Map<IEnumerable<SupplierOrderViewModel>>(await _client.GetAllOrdersByStatusandDate(_mapper.Map<SupplierOrderDTO>(viewModel)));
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

            return Json(new
            {

                success = true,
                message = "Status updated successfully...!",
                data = new
                {
                    Date = model.CreationDate.ToString("dd MMM yyyy, h:mm tt"),
                    CustomerName = model.RestaurantName != null ? model.RestaurantName : "-",
                    CustomerContact = model.RestauantContact != null ? model.RestauantContact : "-",
                    OrderNo = model.OrderNo,
                    Total = model.Amount + model.Currency,
                    OrderRequiredDate = model.OrderRequiredDate.ToString() != "1/1/0001 12:00:00 AM" ? model.OrderRequiredDate.ToString("dd MMM yyyy,h:mm:ss") : "-",
                    Status = model.Status,
                    Id = model.Id

                }

            });
        }
    }
}
