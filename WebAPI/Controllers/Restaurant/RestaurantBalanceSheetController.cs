using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Controllers.Partner;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant")]
	[ApiController]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	public class RestaurantBalanceSheetController : ControllerBase
	{
		private readonly IRestaurantCashierStaffService _RestaurantCashierStaffService;
		private readonly IRestaurantBalanceSheetService _RestaurantBalanceSheetService;
		private readonly IRestaurantPrinterSettingService _printerService;
		private readonly IOrderService _OrderService;
		private readonly IMapper _mapper;
		private readonly IFTPUpload _fTPUpload;
		private readonly IUserService _userService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;
		private readonly ILogger<RestaurantBalanceSheetController> _logger;
		private readonly IMessageService _messageService;

		public RestaurantBalanceSheetController(
			UserManager<AppUser> userManager
			, ILogger<RestaurantBalanceSheetController> logger
			, IMapper mapper
			, IEmailService emailService
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, IRestaurantBalanceSheetService restaurantBalanceSheetService
			, IRestaurantPrinterSettingService printerService
			, IOrderService orderService
			, IUserService userService
			, IMessageService messageService
			, IFTPUpload fTPUpload)
		{
			_userManager = userManager;
			_logger = (ILogger<RestaurantBalanceSheetController>)logger;
			_mapper = mapper;
			_RestaurantCashierStaffService = restaurantCashierStaffService;
			_RestaurantBalanceSheetService = restaurantBalanceSheetService;
			_printerService = printerService;
			_OrderService = orderService;
			_userService = userService;
			_emailService = emailService;
			_messageService = messageService;
			_fTPUpload = fTPUpload;
		}


		[Authorize(Roles = "Admin")]
		[HttpGet("BalanceSheet/ShiftLogs")]
		public async Task<IActionResult> ShiftLogs()
		{
			IEnumerable<RestaurantBalanceSheetDTO> balanceSheets = _mapper.Map<IEnumerable<RestaurantBalanceSheetDTO>>(await _RestaurantBalanceSheetService.GetAllAsync());

			return Ok(new SuccessResponse<IEnumerable<RestaurantBalanceSheetLogsDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantBalanceSheetLogsDTO>>(balanceSheets)));
		}

		[Authorize(Roles = "Admin , RestaurantOwner")]
		[HttpGet("{Id}/BalanceSheet/ShiftLogs")]
		public async Task<IActionResult> ShiftLogs(long Id)
		{
			IEnumerable<RestaurantBalanceSheetDTO> balanceSheets = _mapper.Map<IEnumerable<RestaurantBalanceSheetDTO>>(await _RestaurantBalanceSheetService.GetByRestaurantIdAsync(Id));

			return Ok(new SuccessResponse<IEnumerable<RestaurantBalanceSheetLogsDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantBalanceSheetLogsDTO>>(balanceSheets)));
		}

		[Authorize(Roles = "Admin , RestaurantOwner")]
		[HttpGet("BalanceSheet/{Id}/ShiftLogs")]
		public async Task<IActionResult> ShiftLogDetails(long Id)
		{
			RestaurantBalanceSheetDTO data = _mapper.Map<RestaurantBalanceSheetDTO>(await _RestaurantBalanceSheetService.GetByIdAsync(Id));

			return Ok(new SuccessResponse<RestaurantBalanceSheetLogsDTO>("Data received successfully", _mapper.Map<RestaurantBalanceSheetLogsDTO>(data)));
		}

		[HttpGet("BalanceSheet/GetReport")]
		public async Task<IActionResult> GetReport()
		{
			RestaurantCashierStaffDTO cashier = await GetCurrentStaff();

			RestaurantBalanceSheet balanceSheet = _RestaurantBalanceSheetService.GetByRestaurantCashierStaffIdAsync(cashier.Id).Result.LastOrDefault();

			if (balanceSheet != null)
			{
				//Get Report from repo
				var report = await _RestaurantBalanceSheetService.GetReportByRestaurantCashierStaffIdAsync(cashier.Id, null);

				return Ok(new SuccessResponse<RestaurantBalanceSheetReportDTO>("Data received successfully", report));
			}
			else
			{
				return Ok(new ErrorResponse("Shift not found", null));
			}
		}

		//after closed
		[HttpGet("BalanceSheet/{Id}/GetReport")]
		public async Task<IActionResult> GetReport(long Id)
		{
			var balanceSheet = await _RestaurantBalanceSheetService.GetByIdAsync(Id);

			if (balanceSheet != null)
			{
				RestaurantCashierStaffDTO cashier = await GetCurrentStaff();
				//Get Report from repo
				RestaurantBalanceSheetReportDTO report = await _RestaurantBalanceSheetService.GetReportByRestaurantCashierStaffIdAsync(cashier.Id, Id);

				if (report == null)
				{
					return Ok(new ErrorResponse("Shift not found", null));
				}

				RestaurantPrinterSetting printer = await _printerService.GetByTypeAndRestaurantBranchIdAsync(cashier.RestaurantBranchId, Enum.GetName(typeof(PrinterType), PrinterType.Cashier));
				report.RestaurantPrinterSetting = _mapper.Map<RestaurantPrinterSettingDTO>(printer);
				return Ok(new SuccessResponse<RestaurantBalanceSheetReportDTO>("Data received successfully", report));
			}
			else
			{
				return Ok(new ErrorResponse("Shift not found", null));
			}
		}

		[HttpPost("BalanceSheet/CheckShiftStatus")]
		[ValidateCashier]
		public async Task<IActionResult> CheckShiftStatus(RestaurantBalanceSheetDTO Model)
		{
			var balanceSheets = await _RestaurantBalanceSheetService.GetByRestaurantCashierStaffIdAsync(Model.RestaurantCashierStaffId/*, Model.DeviceId*/);

			var balanceSheet = balanceSheets.LastOrDefault(x => x.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened));

			if (balanceSheet != null)
			{
				return Ok(new SuccessResponse<RestaurantBalanceSheetReportDTO>("Shift opened", _mapper.Map<RestaurantBalanceSheetReportDTO>(balanceSheet)));
			}
			else
			{
				return Ok(new ErrorResponse("Shift closed", null));
			}
		}

		[HttpPost("BalanceSheet/OpenShift")]
		[ValidateCashier]
		public async Task<IActionResult> OpenShift(RestaurantBalanceSheetDTO Model)
		{
			var balanceSheets = await _RestaurantBalanceSheetService.GetByRestaurantCashierStaffIdAsync(Model.RestaurantCashierStaffId/*, Model.DeviceId*/);

			var balanceSheet = balanceSheets.LastOrDefault(x => x.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened));

			if (balanceSheet != null)
			{
				return Ok(new ErrorResponse("Shift already opened", _mapper.Map<RestaurantBalanceSheetReportDTO>(balanceSheet)));
			}
			else
			{
				Model.Status = Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened);
				Model.OpeningDate = DateTime.UtcNow.ToDubaiDateTime();

				return Ok(new SuccessResponse<RestaurantBalanceSheetReportDTO>("Shift opened successfully", _mapper.Map<RestaurantBalanceSheetReportDTO>(await _RestaurantBalanceSheetService.AddRestaurantBalanceSheetAsync(_mapper.Map<RestaurantBalanceSheet>(Model)))));
			}
		}

		[HttpPost("BalanceSheet/CalculateVariance")]
		public async Task<IActionResult> CalculateVariance(RestaurantBalanceSheetDTO Model)
		{
			RestaurantBalanceSheet balanceSheet = await _RestaurantBalanceSheetService.GetByIdAsync(Model.Id);

			if (balanceSheet != null && balanceSheet.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
			{
				//Model.ClosingDate = DateTime.UtcNow.ToDubaiDateTime();
				//balanceSheet = _mapper.Map(Model, balanceSheet);
				balanceSheet.ClosingDate = DateTime.UtcNow.ToDubaiDateTime();
				balanceSheet.ClosingBalance = Model.ClosingBalance;

				//Calculations
				balanceSheet.restaurantCashierStaff.Orders = await _OrderService.GetOrderByCashierStaffID(balanceSheet.RestaurantCashierStaffId, balanceSheet.OpeningDate.Value, balanceSheet.ClosingDate.Value);
				if (balanceSheet.restaurantCashierStaff.Orders.Count > 0)
				{
					balanceSheet.restaurantCashierStaff.Orders = balanceSheet.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();
				}

				RestaurantBalanceSheetDTO balanceSheetDto = _mapper.Map<RestaurantBalanceSheetDTO>(balanceSheet);

				return Ok(new SuccessResponse<object>("Data received successfully", _mapper.Map<RestaurantBalanceSheetVarianceDTO>(balanceSheetDto)));
			}
			else if (balanceSheet != null && balanceSheet.Status != Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
				return Ok(new ErrorResponse("Shift closed", null));
			else
				return Ok(new ErrorResponse("Shift not found", null));
		}

		[HttpPut("BalanceSheet/CloseShift")]
		public async Task<IActionResult> CloseShift(RestaurantBalanceSheetDTO Model)
		{
			RestaurantBalanceSheet balanceSheet = await _RestaurantBalanceSheetService.GetByIdAsync(Model.Id);

			if (balanceSheet != null && balanceSheet.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
			{
				//var orders = balanceSheet.restaurantCashierStaff.Orders;
				//balanceSheet.restaurantCashierStaff.Orders = new List<Order>();
				Model.ClosingDate = DateTime.UtcNow.ToDubaiDateTime();
				Model.Status = Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Closed);

				//add things in balance sheet
				balanceSheet.ClosingDate = Model.ClosingDate;
				balanceSheet.ClosingBalance = Model.ClosingBalance;
				balanceSheet.RestaurantCashDenomination = _mapper.Map<RestaurantCashDenomination>(Model.RestaurantCashDenomination);

				//Model = _mapper.Map(balanceSheet, Model);//get opening balance and other data
				//balanceSheet = _mapper.Map(Model, balanceSheet);//get opening balance and other data
				//balanceSheet = _mapper.Map<RestaurantBalanceSheet>(Model);//get model data

				balanceSheet.Status = Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Closed);
				balanceSheet.restaurantCashierStaff.Orders = await _OrderService.GetOrderByCashierStaffID(balanceSheet.RestaurantCashierStaffId, balanceSheet.OpeningDate.Value, Model.ClosingDate.Value);
				balanceSheet.restaurantCashierStaff.Orders = balanceSheet.restaurantCashierStaff.Orders.Where(x => x.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled)).ToList();

				if (balanceSheet.restaurantCashierStaff.Orders.Count > 0)
				{
					List<Order> orders = balanceSheet.restaurantCashierStaff.Orders.ToList();
					List<OrderDetail> orderDetails = orders.SelectMany(x => x.OrderDetails).ToList();

					/*Aggregator Wise*/
					List<Order> aggregatorOrders = orders.Where(obj => obj.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Aggregator)).ToList();

					foreach (var order in aggregatorOrders)
					{
						balanceSheet.RestaurantAggregatorWiseSales.Add(new RestaurantAggregatorWiseSale()
						{
							RestaurantBalanceSheetId = balanceSheet.Id,
							OrderId = order.Id,
							RestaurantAggregatorId = order.Aggregator.Id,
							PaymentStatus = order.PaidTo,
							Amount = order.TotalAmount, //It may change after testing ... asim
						});
					}

					foreach (var order in orders)
					{
						foreach (var orderDetail in order.OrderDetails)
						{
							/*Product Wise*/
							balanceSheet.RestaurantProductWiseSales.Add(new RestaurantProductWiseSale()
							{
								RestaurantBalanceSheetId = balanceSheet.Id,
								Quantity = orderDetail.Quantity,
								OrderDetailId = orderDetail.Id,
								MenuItemId = orderDetail.MenuItems.Id,
								Amount = orderDetail.TotalPrice, //It may change after testing ... asim
							});

							/*Category Wise*/
							balanceSheet.RestaurantCategoryWiseSales.Add(new RestaurantCategoryWiseSale()
							{
								RestaurantBalanceSheetId = balanceSheet.Id,
								Quantity = orderDetail.Quantity,
								OrderDetailId = orderDetail.Id,
								CategoryId = orderDetail.MenuItems.Category.Id,
								Amount = orderDetail.TotalPrice, //It may change after testing ... asim
							});
						}
					}

					/*Balance Sheet Total*/
					balanceSheet.AggregatorOnlineSale = balanceSheet.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidOnline)).Sum(x => x.Amount);
					balanceSheet.AggregatorCashSale = balanceSheet.RestaurantAggregatorWiseSales.Where(x => x.PaymentStatus == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidToRestaurant)).Sum(x => x.Amount);

					balanceSheet.TotalCashSale = orders.Sum(x => (x.CashReceived - x.Change));

					balanceSheet.TotalCardSale = orders.Sum(x => x.CardAmount);

					balanceSheet.TotalCreditSale = orders.Where(x => x.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit)).Sum(x => x.TotalAmount);

					balanceSheet.GrandTotal = orders.Sum(x => x.TotalAmount);
					balanceSheet.TotalTax = orders.Sum(x => x.TaxAmount);

					balanceSheet.NetTotal = balanceSheet.GrandTotal - balanceSheet.TotalTax;
					balanceSheet.TotalItemQuantity = balanceSheet.RestaurantProductWiseSales.Sum(x => x.Quantity);
					balanceSheet.TotalCategoryCount = balanceSheet.RestaurantCategoryWiseSales.Sum(x => x.Id);
					balanceSheet.DeliveryCharges = orders.Sum(x => x.DeliveryCharges);
					balanceSheet.TotalDiscount = orders.Sum(x => x.DiscountAmount) + orderDetails.Sum(x => x.DiscountAmount);

				}

				//remove things
				balanceSheet.restaurantCashierStaff = null;
				balanceSheet.RestaurantCashDenomination.RestaurantBalanceSheet = null;
				balanceSheet.RestaurantAggregatorWiseSales.ToList().ForEach(x => x.RestaurantAggregator = null);
				balanceSheet.RestaurantProductWiseSales.ToList().ForEach(x => x.MenuItem = null);
				balanceSheet.RestaurantProductWiseSales.ToList().ForEach(x => x.OrderDetail = null);
				balanceSheet.RestaurantCategoryWiseSales.ToList().ForEach(x => x.Category = null);
				balanceSheet.RestaurantCategoryWiseSales.ToList().ForEach(x => x.OrderDetail = null);

				RestaurantBalanceSheet updateSheet = await _RestaurantBalanceSheetService.UpdateRestaurantBalanceSheetAsync(balanceSheet);

				//calculations
				//balanceSheet.restaurantCashierStaff.Orders = orders;
				//balanceSheet = _mapper.Map(Model, balanceSheet);//get model data
				//var resultBalanceSheet = await _RestaurantBalanceSheetService.GetByIdAsync(Model.Id);
				//RestaurantBalanceSheetReportDTO result = _mapper.Map<RestaurantBalanceSheetReportDTO>(resultBalanceSheet);

				//Calculations
				//balanceSheet.restaurantCashierStaff.Orders = await _OrderService.GetOrderByCashierStaffID(balanceSheet.RestaurantCashierStaffId, balanceSheet.OpeningDate.Value, balanceSheet.ClosingDate.Value);
				//RestaurantBalanceSheetDTO balanceSheetDto = _mapper.Map<RestaurantBalanceSheetDTO>(balanceSheet);

				//Update balanceSheet in db
				//RestaurantBalanceSheetReportDTO result = _mapper.Map<RestaurantBalanceSheetReportDTO>(await _RestaurantBalanceSheetService.UpdateRestaurantBalanceSheetAsync(_mapper.Map<RestaurantBalanceSheet>(balanceSheetDto)));

				//var alpha = _mapper.Map<RestaurantBalanceSheet>(balanceSheetDto);
				//RestaurantBalanceSheetReportDTO result = _mapper.Map<RestaurantBalanceSheetReportDTO>(await _RestaurantBalanceSheetService.UpdateRestaurantBalanceSheetAsync(alpha));

				return Ok(new SuccessResponse<RestaurantBalanceSheetReportDTO>("Shift closed successfully", null));
			}
			else if (balanceSheet != null && balanceSheet.Status != Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
				return Ok(new ErrorResponse("Shift already closed", null));
			else
				return Ok(new ErrorResponse("Shift not found", null));
		}

		//get cashier staff
		private async Task<RestaurantCashierStaffDTO> GetCurrentStaff()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<AppUser> list = await _userService.GetUsersByIdAsync(userId);
			AppUser appUser = list.FirstOrDefault();

			RestaurantCashierStaffDTO user = new RestaurantCashierStaffDTO();

			if (appUser.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
			{
				user = _mapper.Map<RestaurantCashierStaffDTO>(_RestaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(appUser.Id).Result);
			}

			return user;
		}
	}

}
