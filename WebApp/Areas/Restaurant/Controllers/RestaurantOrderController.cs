using AutoMapper;
using HelperClasses.DTOs;
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
	public class RestaurantOrderController : Controller
	{
		private readonly IRestaurantOrderClient _client;
		private readonly IRestaurantBranchClient _branchclient;
		private readonly IUserSessionManager _userSessionManager;
		private readonly IRestaurantDeliveryStaffClient _delivertStaffClient;
		private readonly ICustomerTransactionHistoryClient _trasactionClient;
		private readonly IMapper _mapper;
		public RestaurantOrderController(IRestaurantOrderClient client, IUserSessionManager userSessionManager
			, IMapper mapper, IRestaurantBranchClient branchClient
			, IRestaurantDeliveryStaffClient deliveryStaffClient, ICustomerTransactionHistoryClient trasactionClient)
		{
			_client = client;
			_userSessionManager = userSessionManager;
			_mapper = mapper;
			_branchclient = branchClient;
			_delivertStaffClient = deliveryStaffClient;
			_trasactionClient = trasactionClient;
		}

		public async Task<IActionResult> Index()
		{
			long restaurantId = _userSessionManager.GetUserStore().Id;
			var parent = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _branchclient.GetAllRestaurantBranchsAsync(restaurantId));
			ViewBag.RestaurantBranch = new SelectList(parent, "Id", "NameAsPerTradeLicense");
			var Info = _mapper.Map<IEnumerable<RestaurantOrderViewModel>>(await _client.GetAllOrderByRestaurantAsync(restaurantId));
			return View(Info);
		}
		public async Task<IActionResult> List()
		{
			long restaurantId = _userSessionManager.GetUserStore().Id;
			var parent = _mapper.Map<IEnumerable<RestaurantBranchViewModel>>(await _branchclient.GetAllRestaurantBranchsAsync(restaurantId));
			ViewBag.RestaurantBranch = new SelectList(parent, "Id", "NameAsPerTradeLicense");
			var Info = _mapper.Map<IEnumerable<RestaurantOrderViewModel>>(await _client.GetAllOrderByRestaurantAsync(restaurantId));
			return View(Info);
		}

		[HttpPost]
		public async Task<IActionResult> RestaurantBranchList(long branchId, string status)
		{
			long restaurantId = _userSessionManager.GetUserStore().Id;
			RestaurantOrderViewModel viewModel = new RestaurantOrderViewModel();
			viewModel.RestaurantId = restaurantId;
			viewModel.RestaurantBranchId = branchId;
			viewModel.Status = status;
			IEnumerable<RestaurantOrderViewModel> model = _mapper.Map<IEnumerable<RestaurantOrderViewModel>>(await _client.GetAllOrdersByStatus(_mapper.Map<OrderDTO>(viewModel)));
			return PartialView(model);
		}

		public async Task<IActionResult> ChangeStatus(long id)
		{
			RestaurantOrderViewModel model = _mapper.Map<RestaurantOrderViewModel>(await _client.GetOrderByIdAsync(id));
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> ChangeStatus(long OrderId, string Status)
		{
			RestaurantOrderViewModel viewModel = new RestaurantOrderViewModel();
			viewModel.Id = OrderId;
			viewModel.Status = Status;
			viewModel.CustomerId = 0;
			viewModel.DeliveryStaffId = 0;

			RestaurantOrderViewModel model = _mapper.Map<RestaurantOrderViewModel>(await _client.UpdateStatusOrderAsync(_mapper.Map<OrderDTO>(viewModel)));

			return Json(new
			{

				success = true,
				message = "Status updated successfully...!",
				data = new
				{
					Date = model.CreationDate.Value.ToString("dd MMM yyyy, h:mm tt"),
					CustomerName = model.CustomerName != null ? model.CustomerName : "-",
					CustomerContact = model.CustomerContact != null ? model.CustomerContact : "-",
					RestaurantBranchName = model.RestaurantBranch.NameAsPerTradeLicense,
					OrderNo = model.OrderNo,
					Total = model.TotalAmount + model.Currency,
					Status = model.Status,
					Id = model.Id

				}

			});
		}

		public async Task<IActionResult> Details(long id)
		{
			IEnumerable<CustomerTransactionHistoryViewModel> transactrions = _mapper.Map<IEnumerable<CustomerTransactionHistoryViewModel>>(await _trasactionClient.GetTransactionsByOrderIdAsync(id));
			RestaurantOrderViewModel model = _mapper.Map<RestaurantOrderViewModel>(await _client.GetOrderByIdAsync(id));

			model.Transactions = transactrions.OrderByDescending(i => i.Id).ToList();

			return View(model);
		}
		public async Task<IActionResult> AssignDeliveryStaff()
		{
			long restaurantId = _userSessionManager.GetUserStore().Id;
			var parent = _mapper.Map<IEnumerable<RestaurantDeliveryStaffViewModel>>(await _delivertStaffClient.GetAllRestaurantDeliveryStaffsAsync(restaurantId));
			ViewBag.DeliveryStaff = new SelectList(parent, "Id", "FirstName");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AssignDeliveryStaff(long id, long staffId)
		{
			RestaurantOrderViewModel viewModel = _mapper.Map<RestaurantOrderViewModel>(await _client.GetOrderByIdAsync(id));
			viewModel.DeliveryStaffId = staffId;

			RestaurantOrderViewModel model = _mapper.Map<RestaurantOrderViewModel>(await _client.UpdateStatusOrderAsync(_mapper.Map<OrderDTO>(viewModel)));
			if (model != null)
			{
				var staff = await _delivertStaffClient.GetRestaurantDeliveryStaffByIdAsync(staffId);
				return Json(new
				{

					success = true,
					message = "Status updated successfully...!",
					data = new
					{
						name = staff.FirstName,
						contact = staff.PhoneNumber,
						logo = staff.Logo,

					}

				});
			}

			else
			{
				return Json(new
				{

					success = false,
					message = "Something went wrong"


				});
			}

		}

	}
}
