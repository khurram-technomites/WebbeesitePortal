using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Emails;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.POS;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantDeliveryStaff;
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
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/[controller]")]
	[ApiController]
	public class RestaurantOrderController : ControllerBase
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IOrderService _orderService;
		private readonly IRestaurantPrinterSettingService _printerService;
		private readonly IEmailService _emailService;
		private readonly IRestaurantTableReservationService _tableReservationService;
		private readonly IRestaurantTableService _tableService;
		private readonly ICustomerService _customerService;
		private readonly INumberRangeService _numberRangeService;
		private readonly IRestaurantBranchService _restaurantBranchService;
		private readonly IRestaurantCashierStaffService _restaurantCashierStaffService;
		private readonly ICouponService _couponService;
		private readonly ICouponRedemptionService _couponRedemptionService;
		private readonly INotificationService _notificationService;
		private readonly IIntegrationSettingService _integrationSettingService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly ILogger<RestaurantOrderController> _logger;
		public RestaurantOrderController(
			SignInManager<AppUser> signInManager
			, IOrderService orderService
			, IRestaurantPrinterSettingService printerService
			, IMapper mapper
			, IEmailService emailService
			, IRestaurantTableService tableService
			, IRestaurantTableReservationService tableReservationService
			, ILogger<RestaurantOrderController> logger
			, INumberRangeService numberRangeService
			, IRestaurantBranchService restaurantBranchService
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, ICustomerService customerService
			, ICouponService couponService
			, ICouponRedemptionService couponRedemptionService
			, INotificationService notificationService
			, IIntegrationSettingService integrationSettingService
			, IUserService userService
			)
		{
			_signInManager = signInManager;
			_orderService = orderService;
			_printerService = printerService;
			_couponService = couponService;
			_tableService = tableService;
			_tableReservationService = tableReservationService;
			_restaurantBranchService = restaurantBranchService;
			_restaurantCashierStaffService = restaurantCashierStaffService;
			_customerService = customerService;
			_numberRangeService = numberRangeService;
			_emailService = emailService;
			_notificationService = notificationService;
			_integrationSettingService = integrationSettingService;
			_couponRedemptionService = couponRedemptionService;
			_userService = userService;
			_mapper = mapper;
			_logger = logger;
		}


		[HttpGet("GetAll/Restaurants/{restaurantId}")]
		public async Task<IActionResult> GetAll(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByRestaurant(restaurantId)));
		}

		[HttpGet("GetAll/DineIn")]
		public async Task<IActionResult> GetAllDineIn(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByRestaurant(restaurantId)));
		}

		[HttpGet("GetAll/Branch/{branchID}")]
		public async Task<IActionResult> GetAllByBranch(long branchID)
		{
			return Ok(_mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByBranch(branchID)));
		}

		//KDS
		[Authorize(Roles = "RestaurantKitchenManager")]
		[HttpGet("GetAll/Branch/{branchID}/Kds")]
		public async Task<IActionResult> GetAllByBranchKDS(long branchID)
		{
			List<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByBranch(branchID)).Where(x =>
			x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed) ||
			x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing) ||
			x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
			).OrderByDescending(x => x.Id).ToList();

			try
			{
				for (int i = 0; i < orders.Count; i++)
					orders[i].OrderDetails = orders[i].OrderDetails.Where(x => x.Status != Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)).ToList();
			}
			catch (Exception)
			{
			}

			return Ok(orders);
		}

		//KDS
		[HttpGet("GetByBranchAndStatus/Branch/{branchID}")]
		public async Task<IActionResult> GetAllByBranchAndStatus(long branchID, [FromQuery] string type)
		{
			List<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByBranchAndStatus(branchID, type)).ToList();

			try
			{
				for (int i = 0; i < orders.Count; i++)
				{
					orders[i].OrderDetails = orders[i].OrderDetails.Where(x => x.Status != Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)).ToList();

					if (orders[i].OrderDetails.Any(x => x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)))
					{
						Console.Write('d');
					}
				}
			}
			catch (Exception)
			{
			}

			object data = orders.Select(i => new
			{
				OrderId = i.Id,
				CustomerName = i.CustomerName,
				OrderNo = i.OrderNo,
				CreationDateTime = i.CreationDate,
				CreationDate = i.CreationDate.Value.ToString("dd-MM-yyyy"),
				CreationTime = i.CreationDate.Value.ToString("hh:mm"),
				Status = i.Status,
				ItemCount = i.OrderDetails.Sum(j => j.Quantity),
				TotalAmount = i.TotalAmount,
				Detail = i.OrderDetails.Select(x => new
				{

					ItemName = x.MenuItems != null ? x.MenuItems.Name : x.MenuItemName,
					Quantity = x.Quantity,
					CustomerNote = x.CustomerNote == null ? "-" : x.CustomerNote,
					Options = x.OrderDetailOptionValues.GroupBy(x => x.MenuItemOption).Select(grp => grp.FirstOrDefault()).Select(v => new
					{
						OptionName = v.MenuItemOption,
						Values = x.OrderDetailOptionValues.Where(o => o.MenuItemOptionId == v.MenuItemOptionId).Select(i => new
						{
							Name = i.MenuItemOptionValue,
							i.UnitPrice,
							i.Quantity,
							i.Price,
							i.TotalPrice,
						}),

					}),
				}),

			});
			return Ok(new SuccessResponse<object>("Data received successfully", data));
		}

		[HttpGet("GetDetails/Orders/{OrderId}")]
		public async Task<IActionResult> GetDetails(long OrderId)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(OrderId));
			OrderDTO order = orders.FirstOrDefault();

			object data = new
			{
				OrderId = order.Id,
				OrderNo = order.OrderNo,
				CreationDateTime = order.CreationDate,
				CreationDate = order.CreationDate.Value.ToString("dd-MM-yyyy"),
				CreationTime = order.CreationDate.Value.ToString("hh:mm"),
				Status = order.Status,
				ItemCount = order.OrderDetails.Sum(j => j.Quantity),
				DeliveryCharges = order.DeliveryCharges,
				DeliveryType = order.DeliveryType,
				Amount = order.Amount,
				VATAmount = order.TaxAmount,
				VATPercentage = order.TaxPercent,
				CouponCode = order.CouponCode,
				CouponDiscount = order.CouponDiscount,
				TotalAmount = order.TotalAmount,
				EstimatedDeliveryMinutes = order.EstimatedDeliveryMinutes < 0 ? 0 : order.EstimatedDeliveryMinutes,

				//

				DiscountPercent = order.DiscountPercent,
				DiscountAmount = order.DiscountAmount,
				DeliveryDateTime = order.DeliveryDateTime,
				DeliveryDate = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("dd-MM-yyyy") : null,
				DeliveryTime = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("hh:mm") : null,

				CashierStaffId = order.RestaurantCashierStaffId,
				Cashier = order.CashierStaff != null ? order.CashierStaff.FirstName + order.CashierStaff.LastName : null,
				RestaurantWaiterId = order.RestaurantWaiterId,
				RestaurantWaiter = order.RestaurantWaiter != null ? order.RestaurantWaiter.Name : null,
				RestaurantTableId = order.RestaurantTableId,
				Table = order.RestaurantTable != null ? order.RestaurantTable.Name : null,
				PaymentMethod = order.PaymentMethod,
				CardScheme = order.CardScheme != null ? order.CardScheme.Type : null,
				Aggregator = order.Aggregator != null ? order.Aggregator.Name : null,
				PaidTo = order.PaidTo,
				CashReceived = order.CashReceived,
				Change = order.Change,
				CardAmount = order.CardAmount,
				RedeemAmount = order.RedeemAmount,

				IsPaid = order.IsPaid,
				PaidStatus = order.PaidStatus,
				Currency = order.Currency,
				IsCanceled = order.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) ? true : false,
				CancelationReason = order.CancelationReason,
				PaymentRef = order.PaymentRef,
				PaymentCaptured = order.PaymentCaptured,
				IsEarningCaptured = order.IsEarningCaptured,
				OrderRef = order.OrderRef,

				Type = order.Type,
				IsAmended = order.IsAmended,
				AmendReason = order.AmendReason,
				Origin = order.Origin,
				OrderNote = order.OrderNote,

				//

				Detail = order.OrderDetails.Select(x => new
				{

					ItemName = x.MenuItems != null ? x.MenuItems.Name : x.MenuItemName,
					Quantity = x.Quantity,
					CustomerNote = x.CustomerNote == null ? "-" : x.CustomerNote,
					TotalPrice = x.TotalPrice,
					Options = x.OrderDetailOptionValues.GroupBy(x => x.MenuItemOption).Select(grp => grp.FirstOrDefault()).Select(v => new
					{
						OptionName = v.MenuItemOption,
						Values = x.OrderDetailOptionValues.Where(o => o.MenuItemOptionId == v.MenuItemOptionId).Select(i => new
						{
							Name = i.MenuItemOptionValue,
							i.UnitPrice,
							i.Quantity,
							i.Price,
							i.TotalPrice,
						}),
					}),
				}),

				Customer = new
				{
					Id = order.CustomerId,
					Name = order.CustomerName,
					Email = order.CustomerEmail,
					Contact = order.CustomerContact,
					Instruction = order.NoteToRider,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
				},
				Waiter = order.RestaurantWaiter != null ? new
				{
					Id = order.RestaurantWaiter.Id,
					Name = order.RestaurantWaiter.Name,
					Email = order.RestaurantWaiter.Email,
					Contact = order.RestaurantWaiter.Contact,
					Contact2 = order.RestaurantWaiter.Contact2,
					Address = order.RestaurantWaiter.Address,
					Logo = order.RestaurantWaiter.Logo,
				} : null,
				DeliveryStaff = order.DeliveryStaff != null ? new
				{
					Name = order.DeliveryStaff.FirstName + " " + order.DeliveryStaff.LastName,
					Contact = order.DeliveryStaff.PhoneNumber,
					DeliveryCharges = order.DeliveryCharges,
					Latitude = order.Latitude,
					Longitude = order.Longitude,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
					NoteToRider = order.NoteToRider,
					DeliveryStaffCash = order.DeliveryStaffCash,
				} : null,

			};
			return Ok(new SuccessResponse<object>("Data received successfully", data));
		}

		[HttpPost("GetByStatus")]
		public async Task<IActionResult> GetAllByBranchAndStatus(OrderDTO orders)
		{
			return Ok(_mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByBranchAndStatus(orders.RestaurantId, orders.RestaurantBranchId, orders.Status)));
		}

		[HttpGet("GetAll/Restaurants/{restaurantId}/Status/{status}")]
		public async Task<IActionResult> GetAllByRestaurantStatus(long restaurantID, string Status)
		{
			return Ok(_mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByStatus(restaurantID, Status)));
		}

		[HttpGet("OrderType")]
		public IActionResult GetAllOrderType()
		{
			object type = new
			{
				delivery = Enum.GetName(typeof(OrderType), OrderType.Delivery),
				dinein = Enum.GetName(typeof(OrderType), OrderType.DineIn),
				online = Enum.GetName(typeof(OrderType), OrderType.Online),
				takeaway = Enum.GetName(typeof(OrderType), OrderType.Pickup),
			};

			return Ok(new SuccessResponse<object>("Data received successfully", type));
		}

		[HttpGet("OrderStatus")]
		public IActionResult GetAllOrderStatus()
		{
			object status = new
			{
				canceled = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled),
				confirmed = Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed),
				delivered = Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered),
				foodready = Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady),
				notdelivered = Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered),
				ontheway = Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay),
				pending = Enum.GetName(typeof(OrderStatus), OrderStatus.Pending),
				preparing = Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing),
				all = Enum.GetName(typeof(OrderStatus), OrderStatus.All),
			};

			return Ok(new SuccessResponse<object>("Data received successfully", status));
		}

		[HttpPut("ToggleStatus")]
		public async Task<IActionResult> ToggleStatus(OrderDTO orderModel)
		{

			IEnumerable<Order> orderList = await _orderService.GetOrderByIdAsync(orderModel.Id);
			Order order = orderList.FirstOrDefault();

			if (!Enum.IsDefined(typeof(OrderStatus), orderModel.Status))
			{
				return Conflict();
			}
			order.Status = orderModel.Status;

			//Doing this because of entity tracking error. Do not remove!
			List<OrderDetailDTO> orderDetails = _mapper.Map<List<OrderDetailDTO>>(order.OrderDetails);
			order.OrderDetails = null;

			RestaurantDTO restaurant = _mapper.Map<RestaurantDTO>(order.Restaurant);
			order.Restaurant = null;
			order.RestaurantRatings = null;

			OrderDTO model = _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(order));

			model.OrderDetails = orderDetails;

			if (model.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed))
			{
				model.DeliveryStaff = null;
				OrderPlacementEmailDTO emailDTO = new();
				emailDTO.Restaurant = restaurant;
				emailDTO.Order = model;
				emailDTO.Email = order.CustomerEmail;

				await _emailService.SendOrderPlacementEmail(emailDTO);
			}
			else if (model.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered))
				await SendOrderDeliveredEmail(order.CustomerEmail, order.CustomerName, _mapper.Map<Models.Restaurant>(restaurant), order);
			else if (model.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled))
				await SendOrderCancellationEmail(order.CustomerEmail, order.CustomerName, _mapper.Map<Models.Restaurant>(restaurant), order);
			else if (model.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup) && model.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady))
				await SendOrderReadyEmail(model.CustomerEmail, model.CustomerName, restaurant, model);

			return Ok(model);
		}

		private async Task SendOrderDeliveredEmail(string email, string name, Models.Restaurant restaurant, Order order)
		{
			try
			{
				await _emailService.SendGeneralEmail(new GeneralEmailDTO()
				{
					Name = name,
					HTMLBody = $"Your order {order.OrderNo} has been delivered",
					Restaurant = _mapper.Map<RestaurantDTO>(restaurant),
					ButtonLink = string.Format("{0}/tracking?orderId={1}", restaurant.Website, order.OrderNo),
					ButtonText = "Track Order"
				}, "Your order has been delivered", email);

				_logger.LogInformation("Order delivery Email Sent Successfully to " + email);
			}
			catch (Exception ex)
			{
				_logger.LogError("Order delivery Email Failed for " + email + " with message: " +
								  ex.Message);
			}
		}

		private async Task SendOrderCancellationEmail(string email, string name, Models.Restaurant restaurant, Order order)
		{
			try
			{
				await _emailService.SendGeneralEmail(new GeneralEmailDTO()
				{
					Name = name,
					HTMLBody = $"Your order {order.OrderNo} has been cancelled",
					Restaurant = _mapper.Map<RestaurantDTO>(restaurant),
					ButtonLink = string.Format("{0}/tracking?orderId={1}", restaurant.Website, order.OrderNo),
					ButtonText = "View Order Details"
				}, "Your order has been cancelled", email);

				_logger.LogInformation("Order cancellation Email Sent Successfully to " + email);
			}
			catch (Exception ex)
			{
				_logger.LogError("Order cancellation Email Failed for " + email + " with message: " +
								  ex.Message);
			}
		}

		private async Task SendOrderReadyEmail(string email, string name, RestaurantDTO restaurant, OrderDTO order)
		{
			try
			{
				await _emailService.SendGeneralEmail(new GeneralEmailDTO()
				{
					Name = name,
					HTMLBody = $"Your order {order.OrderNo} is ready for pickup",
					Restaurant = restaurant,
					ButtonLink = string.Format("{0}/tracking?orderId={1}", restaurant.Website, order.OrderNo),
					ButtonText = "Track Order"
				}, "Your order is ready for pickup", email);

				_logger.LogInformation("Order cancellation Email Sent Successfully to " + email);
			}
			catch (Exception ex)
			{
				_logger.LogError("Order cancellation Email Failed for " + email + " with message: " +
								  ex.Message);
			}
		}

		#region POS Methods

		/* Slips Starts */

		//Dupliate Slip
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("GetByOrderNo/{OrderNo}")]
		public async Task<IActionResult> GetByIdAsync(string OrderNo)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByOrderNoAsync(OrderNo));
			var order = orders.FirstOrDefault();
			if (order == null)
				return Ok(new ErrorResponse("Order not found", null));
			//else if (order.IsPaid == false)
			//	return Ok(new ErrorResponse("Order payment is pending", null));

			RestaurantPrinterSetting printer = await _printerService.GetByTypeAndRestaurantBranchIdAsync(order.RestaurantBranchId, Enum.GetName(typeof(PrinterType), PrinterType.Cashier));

			object data = GetInvoice(order: order, printer: printer);

			return Ok(new SuccessResponse<object>("Data received successfully !", data));
		}

		//Invoice Slip
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("Invoice/{OrderId}")]
		public async Task<IActionResult> GetInvoice(long OrderId)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(OrderId));
			OrderDTO order = orders.FirstOrDefault();
			RestaurantPrinterSetting printer = await _printerService.GetByTypeAndRestaurantBranchIdAsync(order.RestaurantBranchId, Enum.GetName(typeof(PrinterType), PrinterType.Cashier));

			object data = GetInvoice(order: order, printer: printer);

			return Ok(new SuccessResponse<object>("Data received successfully", data));
		}

		//Kitchen Slip
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("KitchenSlip/{OrderId}")]
		public async Task<IActionResult> GetKitchenSlip(long OrderId)
		{
			var existingOrder = await _orderService.GetOrderByIdAsync(OrderId);

			OrderDTO order = _mapper.Map<OrderDTO>(existingOrder.FirstOrDefault());

			RestaurantPrinterSetting printer = await _printerService.GetByTypeAndRestaurantBranchIdAsync(order.RestaurantBranchId, Enum.GetName(typeof(PrinterType), PrinterType.Kitchen));

			object data = GetKitchenSlip(order: order, printer: printer);

			//update order new, update, canceled status and info
			await UpdateDetailItemsInfo(order: existingOrder.FirstOrDefault());

			return Ok(new SuccessResponse<object>("Data received successfully", data));
		}

		//Pending Payment Slip
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("GetPendingPayment/Branch/{branchID}")]
		public async Task<IActionResult> GetAllBranchPendingPayment(long branchID)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllPendingPaymentsByBranch(branchID)).OrderByDescending(x => x.Id);

			RestaurantPrinterSettingDTO printer = _mapper.Map<RestaurantPrinterSettingDTO>(await _printerService.GetByTypeAndRestaurantBranchIdAsync(branchID, Enum.GetName(typeof(PrinterType), PrinterType.Cashier)));

			IEnumerable<RestaurantPendingPaymentsDTO> pendingPayemnts = _mapper.Map<IEnumerable<RestaurantPendingPaymentsDTO>>(orders);

			object data = new
			{
				DataList = pendingPayemnts.ToList(),
				RestaurantPrinterSetting = printer,
			};

			return Ok(new SuccessResponse<object>("Data received successfully!", data));
		}

		//Delivery Staff Cash Slip
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("DelieryStaffCash/Branch/{branchID}")]
		public async Task<IActionResult> GetAllBranchDelieryStaffCash(long branchID)
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<AppUser> list = await _userService.GetUsersByIdAsync(userId);
			AppUser appUser = list.FirstOrDefault();

			var RestaurantCashierStaff = _restaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(appUser.Id).Result;
			var BalanceSheet = RestaurantCashierStaff.RestaurantBalanceSheets.LastOrDefault(x => x.Status == Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened));

			RestaurantPrinterSettingDTO printer = _mapper.Map<RestaurantPrinterSettingDTO>(await _printerService.GetByTypeAndRestaurantBranchIdAsync(branchID, Enum.GetName(typeof(PrinterType), PrinterType.Cashier)));

			IEnumerable<RestaurantDeliveryStaffCashDTO> cashList = _mapper.Map<IEnumerable<RestaurantDeliveryStaffCashDTO>>(await _orderService.GetAllDeliveryStaffCashByBranch(branchID, BalanceSheet.OpeningDate.Value, BalanceSheet.ClosingDate));

			object data = new
			{
				DataList = cashList.ToList(),
				RestaurantPrinterSetting = printer,
			};

			return Ok(new SuccessResponse<object>("Data received successfully!", data));
		}

		/* Slips Ends */

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("UpdateDetailStatus/{OrderId}")]
		public async Task<IActionResult> UpdateDetailStatus(long OrderId)
		{
			var existingOrder = await _orderService.GetOrderByIdAsync(OrderId);

			//update order new, update, canceled status and info
			await UpdateDetailItemsInfo(order: existingOrder.FirstOrDefault());

			return Ok(new SuccessResponse<object>("Data updated successfully", null));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPost("VerifySupervisor")]
		public async Task<IActionResult> VerifySupervisor(SupervisorVerificationDTO model)
		{
			IEnumerable<AppUser> Users = await _userService.GetUserByNumber(model.PhoneNumber);
			AppUser user = Users.FirstOrDefault();

			if (!Users.Any() || Users.FirstOrDefault().IsDeleted)
				return Ok(new ErrorResponse("No user found. Invalid phone number", null));

			if (!Users.FirstOrDefault().PhoneNumberConfirmed)
				return Conflict(new ErrorResponse("Kindly confirm phone number first", null));

			if (!Users.FirstOrDefault().IsActive)
				return Conflict(new ErrorResponse("Account suspended. Contact your Administrator", null));

			if (user.LoginFor != Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager))
				return Conflict(new ErrorResponse("Cannot proceed the request with provided actor", null));

			var result = await _signInManager.PasswordSignInAsync(Users.FirstOrDefault(), model.Password, false, false);

			if (result.Succeeded)
			{
				object response = new
				{
					IsSupervisorVerified = true
				};
				return Ok(new SuccessResponse<object>("Supervisor verified successfully", response));
			}

			return Ok(new ErrorResponse("Invalid password ", null));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("GetAll/Branch/{branchID}/Pos")]
		public async Task<IActionResult> GetAllBranchPOS(long branchID)
		{
			IEnumerable<OrderDTO> dto = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByBranchPos(branchID)).OrderByDescending(x => x.Id);

			try
			{
				for (int i = 0; i < dto.ToList().Count(); i++)
				{
					dto.ToList()[i].OrderDetails = dto.ToList()[i].OrderDetails.Where(x => x.Status != Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)).ToList();
				}
			}
			catch (Exception)
			{
			}

			object Orders = new
			{
				DineIn = dto.Where(x => x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.DineIn)
				&& x.IsOnline == false
				&&
				(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)
				)
				).ToList(),

				PickUp = dto.Where(x => x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup)
				&& x.IsOnline == false
				&&
				(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)
				)
				).ToList(),

				Delivery = dto.Where(x => x.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery)
				&& x.IsOnline == false
				&&
				(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)
				|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)
				)
				).ToList(),

				//Online = dto.Where(x => x.IsOnline == true
				//&&
				//(x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Pending)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Preparing)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.OnTheWay)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered)
				//|| x.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered)
				//)
				//).ToList(),

			};

			return Ok(new SuccessResponse<object>("Data received successfully!", Orders));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetByIdAsync(long Id)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(Id));

			return Ok(new SuccessResponse<OrderDTO>("Data received successfully !", orders.FirstOrDefault()));
		}

		//Get Customer By Order Id
		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpGet("GetCustomer/{Id}")]
		public async Task<IActionResult> GetCustomerByIdAsync(long Id)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(Id));

			OrderDTO order = orders.FirstOrDefault();

			if (order != null && order.CustomerId != null)
			{
				IEnumerable<CustomerDTO> result = _mapper.Map<IEnumerable<CustomerDTO>>(await _customerService.GetByIdAsync((long)order.CustomerId));

				if (result.Any())
				{
					return Ok(new SuccessResponse<CustomerDTO>("Data received successfully !", result.FirstOrDefault()));
				}
			}

			return Ok(new ErrorResponse("This order doesn't have any customer !", null));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPost("PlaceOrder")]
		public async Task<IActionResult> PlaceOrder(OrderPlacementDTO orderPlacementDTO)
		{
			List<NotificationReceiver> notificationReceivers = new();
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(orderPlacementDTO.RestaurantBranchId);
			RestaurantBranch branch = branches.FirstOrDefault();

			orderPlacementDTO.RestaurantCashierStaffId = branch.RestaurantCashierStaffs.FirstOrDefault(x => x.UserId == UserId).Id;

			Order order = new();

			//Dine In
			if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.DineIn))
			{
				if (orderPlacementDTO.RestaurantWaiterId == null)
					return Ok(new ErrorResponse("Waiter staff is not assigned!", null));

				if (orderPlacementDTO.TableId == null)
					return Ok(new ErrorResponse("Table id is compulsory when type is Dine In", null));

				if (!/*string.IsNullOrEmpty(orderPlacementDTO.MergeTableIds)*/true)
				{
					var Ids = orderPlacementDTO.MergeTableIds.Split(",");

					for (int i = 0; i <= Ids.Length; i++)
					{
						RestaurantTableDTO table = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync(Convert.ToInt64(Ids[i])));
						if (table.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
							return Ok(new ErrorResponse("Table is reserved", null));
					}
				}
				else
				{
					RestaurantTableDTO table = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync((long)orderPlacementDTO.TableId));
					if (table.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
						return Ok(new ErrorResponse("Table is reserved", null));
				}

				//order.MergeTableIds = orderPlacementDTO.MergeTableIds;
				//order.RestaurantTableId = orderPlacementDTO.TableId;
			}

			//Delivery
			else if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery))
			{
				if (orderPlacementDTO.CustomerId == null || orderPlacementDTO.Latitude == 0 || orderPlacementDTO.Longitude == 0 || string.IsNullOrEmpty(orderPlacementDTO.Address))
					return Ok(new ErrorResponse("Delivery details are missing!", null));

				if (orderPlacementDTO.DeliveryStaffId == null)
					return Ok(new ErrorResponse("Delivery staff is not assigned!", null));
			}
			//Pickup
			else if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup))
			{

			}
			order = _mapper.Map<Order>(orderPlacementDTO);
			order.OrderNo = await _numberRangeService.GetNumberRangeByName("ORDER");
			//order.TaxPercent = branches.FirstOrDefault().Restaurant.TaxPercent;
			//order.TaxAmount = 0;

			//order.DiscountPercent = orderDTO.DiscountPercentage;
			//order.DiscountAmount = orderDTO.DiscountAmount;
			//order.TotalAmount = orderDTO.TotalAmount;
			//order.Amount = orderDTO.Subtotal;
			//order.DeliveryCharges = orderDTO.DeliveryCharges;
			//order.TaxAmount = orderDTO.TaxAmount;
			//order.TaxPercent = orderDTO.TaxPercentage;
			//order.DiscountPercent = 0;
			order.RedeemAmount = 0;
			order.Status = Enum.GetName(typeof(Status), Status.Pending);
			//order.Currency = "AED";
			order.OrderPlacementDateTime = DateTime.UtcNow.ToDubaiDateTime();
			order.RestaurantId = branch.RestaurantId;
			order.EstimatedDeliveryMinutes = branch.DeliveryMinutes;
			order.DeliveryDateTime = DateTime.UtcNow.ToDubaiDateTime();
			//order.CashReceived = orderDTO.CardReceived;
			//order.Change = orderDTO.Change;
			//order.CardSchemeId = orderDTO.CardSchemeId;
			order.PaymentMethod = Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Pending);
			//order.DeliveryType = orderDTO.DeliveryType;
			order.CustomerContact = !string.IsNullOrEmpty(order.CustomerContact) ? order.CustomerContact.Replace("+", "") : "";

			////Do not remove this
			//if (order.CustomerId != null && order.CustomerId != 0)
			//{
			//	//if (!order.CustomerContact.StartsWith("+"))
			//	//{
			//	//	order.CustomerContact = "+" + order.CustomerContact;
			//	//}
			//	//else
			//	//{
			//	order.CustomerContact = order.CustomerContact.Replace("+", "");
			//	//}

			//	order.CustomerName = order.CustomerName;
			//	order.CustomerEmail = order.CustomerEmail;
			//	order.CustomerId = order.CustomerId;
			//}

			//foreach (var Detail in order.OrderDetails)
			//{
			//    //Detail.Price += Detail.OrderDetailOptionValues.Sum(x => x.TotalPrice);
			//    //Detail.TotalPrice = Detail.Quantity * Detail.Price;
			//}

			//if (branch != null && order.TotalAmount < branch.MinOrderPrice && orderPlacementDTO.Origin != Enum.GetName(typeof(OrderOrigin), OrderOrigin.POS))
			//	return Conflict(new ErrorDetails(409, string.Format("You must have a minimum order amount of {0} to place your order. Your current order total is {1}", branch.MinOrderPrice, order.Amount), null));

			if (order.OrderDetails.FirstOrDefault(x => x.IsUpdated == true) != null)
				foreach (var item in order.OrderDetails.Where(x => x.IsUpdated == true))
					item.IsUpdated = false;

			OrderDTO orderDto = _mapper.Map<OrderDTO>(await _orderService.AddOrderAsync(order));
			RestaurantTableReservationDTO reservation = new RestaurantTableReservationDTO();
			if (orderPlacementDTO.TableId != null)
			{
				reservation.OrderId = orderDto.Id;
				reservation.RestaurantTableId = (long)orderPlacementDTO.TableId;
				reservation.MergeTableIds = orderDto.MergeTableIds;
				reservation.Note = orderDto.NoteToRider;
				reservation.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved);
				reservation.ReservationDate = DateTime.UtcNow.ToDubaiDateTime();
				reservation.ReservationTime = reservation.ReservationDate.Value.TimeOfDay;

				if (order.CustomerId != null && order.CustomerId != 0)
				{
					reservation.Name = order.CustomerName;
					reservation.Contact = !string.IsNullOrEmpty(order.CustomerContact) ? order.CustomerContact.Replace("+", "") : "";
				}
				else
				{
					reservation.Name = "";
					reservation.Contact = "";
				}

				await _tableReservationService.AddRestaurantTableReservationByOrder(_mapper.Map<RestaurantTableReservation>(reservation));
			}

			return Ok(new SuccessResponse<OrderDTO>("Order placed successfully", orderDto));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPut("UpdateOrder")]
		public async Task<IActionResult> UpdateOrder(OrderPlacementDTO orderPlacementDTO)
		{
			//List<NotificationReceiver> notificationReceivers = new();
			//string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(orderPlacementDTO.RestaurantBranchId);
			RestaurantBranch branch = branches.FirstOrDefault();

			IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync((long)orderPlacementDTO.OrderId);
			Order oldOrder = orders.FirstOrDefault();
			oldOrder.RestaurantTable = null;
			oldOrder.Customer = null;

			RestaurantTableDTO oldTable = new();
			if (oldOrder.RestaurantTableId != null && oldOrder.RestaurantTableId > 0)
			{
				oldTable = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync((long)oldOrder.RestaurantTableId));
			}

			/* Restaurant Table flow start */

			if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.DineIn))
			{
				//check Waiter Staff
				if (orderPlacementDTO.RestaurantWaiterId == null)
					return Ok(new ErrorResponse("Waiter staff id is compulsory when type is Dine In", null));

				//remove Delivery Staff
				oldOrder.DeliveryStaffId = null;
				orderPlacementDTO.DeliveryStaffId = null;

				if (orderPlacementDTO.TableId != null && orderPlacementDTO.TableId != oldTable.Id)
				{
					if (orderPlacementDTO.TableId == null)
						return Ok(new ErrorResponse("Table id is compulsory when type is Dine In", null));

					if (!/*string.IsNullOrEmpty(orderPlacementDTO.MergeTableIds)*/true)
					{
						var Ids = orderPlacementDTO.MergeTableIds.Split(",");

						for (int i = 0; i <= Ids.Length; i++)
						{
							RestaurantTableDTO table = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync(Convert.ToInt64(Ids[i])));
							if (table.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
								return Ok(new ErrorResponse("Table is reserved", null));
						}
					}
					else
					{
						RestaurantTableDTO table = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync((long)orderPlacementDTO.TableId));
						if (table.Status == Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved))
							return Ok(new ErrorResponse("Table is reserved", null));
					}

					oldOrder.MergeTableIds = orderPlacementDTO.MergeTableIds;
					oldOrder.RestaurantTableId = orderPlacementDTO.TableId;
				}

			}
			else if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery))
			{
				//remove Waiter
				oldOrder.RestaurantWaiterId = null;
				orderPlacementDTO.RestaurantWaiterId = null;
				//check Delivery Staff
				if (orderPlacementDTO.DeliveryStaffId == null)
					return Ok(new ErrorResponse("Delivery staff id is compulsory when type is Delivery", null));

				if (orderPlacementDTO.CustomerId == null
					|| orderPlacementDTO.Latitude == 0
					|| orderPlacementDTO.Longitude == 0
					|| string.IsNullOrEmpty(orderPlacementDTO.Address))
					return Ok(new ErrorResponse("Delivery details are missing!", null));

				//remove table
				oldOrder.RestaurantTableId = null;
				orderPlacementDTO.TableId = null;
			}
			else if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup))
			{
				//remove Waiter
				oldOrder.RestaurantWaiterId = null;
				orderPlacementDTO.RestaurantWaiterId = null;

				//remove Delivery Staff
				oldOrder.DeliveryStaffId = null;
				orderPlacementDTO.DeliveryStaffId = null;

				//remove table
				oldOrder.RestaurantTableId = null;
				orderPlacementDTO.TableId = null;

			}
			else if (orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Online))
			{
				//remove Waiter
				oldOrder.RestaurantWaiterId = null;
				orderPlacementDTO.RestaurantWaiterId = null;

				//remove table
				oldOrder.RestaurantTableId = null;
				orderPlacementDTO.TableId = null;
			}
			else
			{
				return Ok(new ErrorResponse("Delivery Type is not defined!", null));
			}

			/* Restaurant Table flow end */

			oldOrder = _mapper.Map(orderPlacementDTO, oldOrder);
			oldOrder.Status = Enum.GetName(typeof(Status), Status.Pending);
			oldOrder.CustomerContact = !string.IsNullOrEmpty(oldOrder.CustomerContact) ? oldOrder.CustomerContact.Replace("+", "") : "";

			//oldOrder.IsAmended = true;

			oldOrder.RestaurantWaiter = null;
			oldOrder.DeliveryStaff = null;
			oldOrder.RestaurantCashierStaff = null;
			oldOrder.Restaurant = null;
			oldOrder.RestaurantBranch = null;

			if (oldOrder.RestaurantCashierStaffId == 0 || oldOrder.RestaurantCashierStaffId == null)
			{
				string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				orderPlacementDTO.RestaurantCashierStaffId = branch.RestaurantCashierStaffs.FirstOrDefault(x => x.UserId == UserId).Id;
			}

			oldOrder.EditCount = oldOrder.EditCount + 1;
			oldOrder.IsOnline = false;
			OrderDTO orderDto = _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(oldOrder));

			/* Restaurant Table flow start */
			if ((orderPlacementDTO.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.DineIn)))
			{
				RestaurantTableReservationDTO reservation = new RestaurantTableReservationDTO();

				if ((orderPlacementDTO.TableId != null && orderPlacementDTO.TableId != oldTable.Id))
				{
					orderDto.RestaurantTableId = (long)orderPlacementDTO.TableId;
					if (oldTable.Id > 0)
					{
						oldTable.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
						await _tableReservationService.UpdateRestaurantTableReservationByOrder(_mapper.Map<RestaurantTable>(oldTable), oldOrder.Id);
					}

					reservation.OrderId = orderDto.Id;
					reservation.RestaurantTableId = (long)orderPlacementDTO.TableId;
					reservation.MergeTableIds = orderDto.MergeTableIds;
					reservation.Note = orderDto.NoteToRider;
					reservation.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Reserved);
					reservation.ReservationDate = DateTime.UtcNow.ToDubaiDateTime();
					reservation.ReservationTime = reservation.ReservationDate.Value.TimeOfDay;

					if (oldOrder.CustomerId != null && oldOrder.CustomerId != 0)
					{
						reservation.Name = oldOrder.CustomerName;
						reservation.Contact = !string.IsNullOrEmpty(oldOrder.CustomerContact) ? oldOrder.CustomerContact.Replace("+", "") : "";
					}

					await _tableReservationService.AddRestaurantTableReservationByOrder(_mapper.Map<RestaurantTableReservation>(reservation));
				}

			}
			else if (oldTable.Id > 0)
			{
				oldTable.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
				await _tableReservationService.UpdateRestaurantTableReservationByOrder(_mapper.Map<RestaurantTable>(oldTable), oldTable.Id);

			}

			/* Restaurant Table flow end*/
			orderDto.OrderDetails = orderDto.OrderDetails.Where(x => x.Status != Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)).ToList();

			return Ok(new SuccessResponse<OrderDTO>("Order updated successfully", orderDto));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPut("proceedpayment")]
		public async Task<IActionResult> ProceedPayment(ProceedPaymentDTO payment)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(payment.OrderId));
			OrderDTO order = orders.FirstOrDefault();
			if (order == null)
				return Ok(new ErrorResponse("Order not found", null));
			if (payment.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit))
			{
				if (payment.CustomerId == null)
				{
					return Ok(new ErrorResponse("Customer Id is neccessory when you select credit", null));
				}
				Customer customer = new();

				IEnumerable<Customer> customers = await _customerService.GetByIdAsync(payment.CustomerId.Value);
				customer = customers.LastOrDefault();

				if (customer != null)
				{
					order.CustomerName = customer.Name;
					order.CustomerContact = !string.IsNullOrEmpty(customer.Contact) ? customer.Contact.Replace("+", "") : "";
				}
			}
			else if (payment.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Aggregator))
			{
				if (payment.AggregatorId == null)
				{
					return Ok(new ErrorResponse("Aggregator Id is neccessory when you select aggregator", null));
				}
				order.AggregatorId = payment.AggregatorId;
				order.PaidTo = payment.PaidTo;
				if (order.PaidTo == Enum.GetName(typeof(PaymentStatus), PaymentStatus.PaidToRestaurant))
				{
					order.CashReceived = payment.CashReceived.Value;
					order.Change = payment.Change.Value;
				}
				else
				{
					order.CardAmount = payment.CardAmount.Value;
				}
			}
			else if (payment.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card))
			{
				if (payment.CardId == null)
				{
					return Ok(new ErrorResponse("Card Id is neccessory when you select card", null));
				}
				order.CardSchemeId = payment.CardId;
				order.CardAmount = payment.FinalBillAmount;
			}
			else if (payment.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Partial))
			{
				if (payment.CardId == null)
				{
					return Ok(new ErrorResponse("Card Id is neccessory when you select partial", null));
				}
				order.CardSchemeId = payment.CardId;
				order.CardAmount = payment.CardAmount.Value;
				order.CashReceived = payment.CashReceived.Value;
				order.Change = payment.Change.Value;
			}
			else if (payment.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Cash))
			{
				order.CashReceived = payment.CashReceived.Value;
				order.Change = payment.Change.Value;
			}
			else
			{
				return Ok(new ErrorResponse("Payment method is not defined", null));
			}
			order.TotalAmount = payment.FinalBillAmount;
			order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered);
			order.IsPaid = true;

			//if (payment.CustomerId != null && payment.PaymentMethod != Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Credit))
			//{
			//	order.CustomerId = payment.CustomerId;
			//	order.CustomerName = payment.CustomerName;
			//	order.CustomerContact = !string.IsNullOrEmpty(payment.CustomerContact) ? payment.CustomerContact.Replace("+", "") : "";
			//}

			order.PaymentMethod = payment.PaymentMethod;
			order.Customer = null;
			order.Restaurant = null;
			order.RestaurantBranch = null;
			order.RestaurantRatings = null;
			order.CashierStaff = null;
			order.Aggregator = null;
			order.RestaurantTableId = order.RestaurantTableId == 0 ? null : order.RestaurantTableId;
			Order currentOrder = _mapper.Map<Order>(order);
			foreach (var item in currentOrder.OrderDetails)
			{
				item.MenuItems = null;
			}

			OrderDTO result = _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(currentOrder));

			//Un Assign Table
			if (order.RestaurantTableId != null)
			{
				var oldTable = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync((long)order.RestaurantTableId));
				oldTable.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
				await _tableReservationService.UpdateRestaurantTableReservationByOrder(_mapper.Map<RestaurantTable>(oldTable), order.Id);
			}

			return Ok(new SuccessResponse<OrderDTO>("Order payment completed successfully", result));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPut("CancelOrder/{OrderId}")]
		public async Task<IActionResult> CancelOrder(long OrderId, [FromQuery] string Reason)
		{
			IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync(OrderId);

			Order order = orders.FirstOrDefault();

			order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
			order.CancelationReason = Reason;

			order.OrderDetails = null;
			order.RestaurantBranch = null;
			order.Restaurant = null;
			order.Customer = null;
			order.DeliveryStaff = null;
			order.RestaurantRatings = null;
			order.RestaurantTable = null;
			order.RestaurantCashierStaff = null;

			Order result = await _orderService.UpdateOrderAsync(order);

			//Remove Table
			if (order.RestaurantTableId != null && order.RestaurantTableId > 0)
			{
				var Table = _mapper.Map<RestaurantTableDTO>(await _tableService.GetByIdAsync((long)order.RestaurantTableId));
				Table.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Canceled);
				await _tableReservationService.UpdateRestaurantTableReservationByOrder(_mapper.Map<RestaurantTable>(Table), order.Id);
				Table.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);
			}

			return Ok(new SuccessResponse<OrderDTO>("Order canceled successfully", _mapper.Map<OrderDTO>(result)));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPut("amendOrder/{OrderNumeber}")]
		public async Task<IActionResult> AmendOrder(string OrderNumeber, [FromQuery] string Reason)
		{
			IEnumerable<OrderDTO> orders = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByOrderNoAsync(OrderNumeber));
			OrderDTO order = orders.FirstOrDefault();

			if (order == null)
				return Ok(new ErrorResponse("Order not found", null));

			order.AggregatorId = null;
			order.PaidTo = "";
			order.CardAmount = 0;
			order.CardSchemeId = null;
			order.CashReceived = 0;
			order.Change = 0;

			order.Amount = 0;
			order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Pending);
			order.IsPaid = false;
			order.PaidStatus = Enum.GetName(typeof(OrderPaidStatus), OrderPaidStatus.UnPaid);

			order.IsAmended = true;
			order.AmendReason = Reason;

			order.PaymentMethod = Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Pending);
			order.Customer = null;
			order.Restaurant = null;
			order.RestaurantBranch = null;
			order.RestaurantRatings = null;
			order.CashierStaff = null;
			order.Aggregator = null;
			order.RestaurantTableId = order.RestaurantTableId == 0 ? null : order.RestaurantTableId;
			Order currentOrder = _mapper.Map<Order>(order);
			foreach (var item in currentOrder.OrderDetails)
			{
				item.MenuItems = null;
			}

			OrderDTO result = _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(currentOrder));

			return Ok(new SuccessResponse<OrderDTO>("Order amended successfully", result));
		}

		[Authorize(Roles = "RestaurantOwner, RestaurantCashierStaff")]
		[HttpPut("MergeTable/{Id1}/OrderTable/{Id2}")]
		public async Task<IActionResult> MergeTable(long Id1, long Id2)
		{
			RestaurantTable table1 = new(), table2 = new();
			RestaurantTableReservation reservation1 = new(), reservation2 = new();
			Order order1 = new(), order2 = new();

			#region Get Tables by IDs

			table1 = await _tableService.GetByIdAsync(Id1);
			reservation1 = table1.RestaurantTableReservations.OrderByDescending(x => x.Id).FirstOrDefault();

			table2 = await _tableService.GetByIdAsync(Id2);
			reservation2 = table2.RestaurantTableReservations.OrderByDescending(x => x.Id).FirstOrDefault();

			#endregion

			#region Get Orders by Table IDs

			//1st Order | Source Order
			IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync((long)reservation1.OrderId);
			order1 = orders.FirstOrDefault();

			//2nd Order | Destination Order
			orders = await _orderService.GetOrderByIdAsync((long)reservation2.OrderId);
			order2 = orders.FirstOrDefault();

			#endregion

			if (order1 != null && order2 != null)
			{
				#region Update Order 2

				order2.Amount = order1.Amount + order2.Amount;
				order2.TotalAmount = order1.TotalAmount + order2.TotalAmount;
				//order2.OrderRef = order1.OrderNo;
				order2.MergeTableIds = $"{Id1},{Id2}";

				foreach (var detail in order1.OrderDetails)
				{
					detail.OrderId = order2.Id;
					order2.OrderDetails.Add(detail);
				}

				#endregion

				#region Cancel Order 1

				order1.OrderRef = order2.OrderNo;
				order1.MergeTableIds = $"{Id1},{Id2}";
				order1.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
				order1.CancelationReason = $"Tables Merged | Order {order1.OrderNo} is merged with {order2.OrderNo}";

				order1.OrderDetails = new List<OrderDetail>();
				order1.RestaurantBranch = null;
				order1.Restaurant = null;
				order1.Customer = null;
				order1.DeliveryStaff = null;
				order1.RestaurantRatings = null;
				order1.RestaurantTable = null;
				order1.RestaurantCashierStaff = null;
				order1.RestaurantWaiter = null;
				order1.RestaurantRatings = null;

				#endregion

				#region Result

				Order result1 = await _orderService.UpdateOrderAsync(order1); // cancel order 1

				if (result1 != null && result1.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled))
				{

					//order2.OrderDetails = new List<OrderDetail>();
					order2.RestaurantBranch = null;
					order2.Restaurant = null;
					order2.Customer = null;
					order2.DeliveryStaff = null;
					order2.RestaurantRatings = null;
					order2.RestaurantTable = null;
					order2.RestaurantCashierStaff = null;
					order2.RestaurantWaiter = null;
					order2.RestaurantRatings = null;
					order2.OrderDetails.ToList().ForEach(x => x.Category = null);
					order2.OrderDetails.ToList().ForEach(x => x.MenuItems = null);
					order2.OrderDetails.ToList().ForEach(x => x.Order = null);

					Order result2 = await _orderService.UpdateOrderAsync(order2);

					#region Table Release Flow

					//Remove 1st Table
					table1.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Merged);
					await _tableReservationService.UpdateRestaurantTableReservationByOrder(table1, order1.Id);
					//table1.Status = Enum.GetName(typeof(TableReservationStatus), TableReservationStatus.Active);

					#endregion
				}

				#endregion
			}
			else
			{
				return Ok(new ErrorResponse("Orders not found !", null));
			}

			return Ok(new SuccessResponse<OrderDTO>("Tables merged successfully", null));
		}


		#endregion

		#region Private Methods

		//Get Invoice Object
		private static object GetInvoice(OrderDTO order, RestaurantPrinterSetting printer)
		{
			object result = new();

			#region result set

			result = new
			{
				OrderId = order.Id,
				OrderNo = order.OrderNo,
				CreationDateTime = order.CreationDate,
				CreationDate = order.CreationDate.Value.ToString("dd-MM-yyyy"),
				CreationTime = order.CreationDate.Value.ToString("hh:mm"),
				OrderPlacementDate = order.OrderPlacementDateTime.HasValue ? order.OrderPlacementDateTime.Value.ToString("dd-MM-yyyy") : null,
				OrderPlacementTime = order.OrderPlacementDateTime.HasValue ? order.OrderPlacementDateTime.Value.ToString("hh:mm") : null,
				FoodReadyDate = order.FoodReadyDateTime.HasValue ? order.FoodReadyDateTime.Value.ToString("dd-MM-yyyy") : null,
				FoodReadyTime = order.FoodReadyDateTime.HasValue ? order.FoodReadyDateTime.Value.ToString("hh:mm") : null,
				PickedByRiderDate = order.PickedByRiderDateTime.HasValue ? order.PickedByRiderDateTime.Value.ToString("dd-MM-yyyy") : null,
				PickedByRiderTime = order.PickedByRiderDateTime.HasValue ? order.PickedByRiderDateTime.Value.ToString("hh:mm") : null,

				Status = order.Status,
				ItemCount = order.OrderDetails.Sum(j => j.Quantity),
				DeliveryCharges = order.DeliveryCharges,
				DeliveryType = order.DeliveryType,
				Amount = order.Amount,
				VATAmount = order.TaxAmount,
				VATPercentage = order.TaxPercent,

				DiscountPercent = order.DiscountPercent,
				DiscountAmount = order.DiscountAmount,

				CouponCode = order.CouponCode,
				CouponDiscount = order.CouponDiscount,
				TotalAmount = order.TotalAmount,

				DeliveryDateTime = order.DeliveryDateTime,
				DeliveryDate = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("dd-MM-yyyy") : null,
				DeliveryTime = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("hh:mm") : null,
				EstimatedDeliveryMinutes = order.EstimatedDeliveryMinutes < 0 ? 0 : order.EstimatedDeliveryMinutes,

				CashierStaffId = order.RestaurantCashierStaffId,
				Cashier = order.CashierStaff != null ? order.CashierStaff.FirstName + order.CashierStaff.LastName : null,
				RestaurantWaiterId = order.RestaurantWaiterId,
				RestaurantWaiter = order.RestaurantWaiter != null ? order.RestaurantWaiter.Name : null,
				RestaurantTableId = order.RestaurantTableId,
				Table = order.RestaurantTable != null ? order.RestaurantTable.Name : null,
				PaymentMethod = order.PaymentMethod,
				CardScheme = order.CardScheme != null ? order.CardScheme.Type : null,
				Aggregator = order.Aggregator != null ? order.Aggregator.Name : null,
				PaidTo = order.PaidTo,
				CashReceived = order.CashReceived,
				Change = order.Change,
				CardAmount = order.CardAmount,
				RedeemAmount = order.RedeemAmount,

				IsPaid = order.IsPaid,
				PaidStatus = order.PaidStatus,
				Currency = order.Currency,
				IsCanceled = order.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) ? true : false,
				CancelationReason = order.CancelationReason,
				PaymentRef = order.PaymentRef,
				PaymentCaptured = order.PaymentCaptured,
				IsEarningCaptured = order.IsEarningCaptured,
				OrderRef = order.OrderRef,

				Type = order.Type,
				IsAmended = order.IsAmended,
				AmendReason = order.AmendReason,
				Origin = order.Origin,
				OrderNote = order.OrderNote,

				IsPrinterAllowed = order.CashierStaff != null && order.CashierStaff.IsPrinterAllowed,
				EditCount = order.EditCount,
				IsUpdated = order.EditCount > 1 ? true : false,

				Detail = order.OrderDetails.Any() ? order.OrderDetails.Where(x => x.Status != Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled)).Select(x => new
				{
					ItemName = x.MenuItems != null ? x.MenuItems.Name : x.MenuItemName,
					Quantity = x.Quantity,
					CustomerNote = x.CustomerNote == null ? "-" : x.CustomerNote,
					UnitPrice = x.UnitPrice,
					Price = x.Price,
					TotalPrice = x.TotalPrice,

					TaxPercent = x.TaxPercent,
					TaxAmount = x.TaxAmount,
					DiscountPercent = x.DiscountPercent,
					DiscountAmount = x.DiscountAmount,

					Status = x.Status,
					IsCanceled = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled) ? true : false,
					//IsUpdated = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Updated) ? true : false,
					IsUpdated = x.EditCount > 1 ? true : false,
					EditCount = x.EditCount,

					Options = x.OrderDetailOptionValues.GroupBy(x => x.MenuItemOption).Select(grp => grp.FirstOrDefault()).Select(v => new
					{
						OptionName = v.MenuItemOption,
						Values = x.OrderDetailOptionValues.Where(o => o.MenuItemOptionId == v.MenuItemOptionId).Select(i => new
						{
							Name = i.MenuItemOptionValue,
							i.UnitPrice,
							i.Quantity,
							i.Price,
							i.TotalPrice,
						}),

					}),
				}) : null,

				Customer = new
				{
					Id = order.CustomerId,
					Name = order.CustomerName,
					Email = order.CustomerEmail,
					Contact = order.CustomerContact,
					Instruction = order.NoteToRider,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
				},
				Waiter = order.RestaurantWaiter != null ? new
				{
					Id = order.RestaurantWaiter.Id,
					Name = order.RestaurantWaiter.Name,
					Email = order.RestaurantWaiter.Email,
					Contact = order.RestaurantWaiter.Contact,
					Contact2 = order.RestaurantWaiter.Contact2,
					Address = order.RestaurantWaiter.Address,
					Logo = order.RestaurantWaiter.Logo,
				} : null,
				DeliveryStaff = order.DeliveryStaff != null ? new
				{
					Name = order.DeliveryStaff.FirstName + " " + order.DeliveryStaff.LastName,
					Contact = order.DeliveryStaff.PhoneNumber,
					DeliveryCharges = order.DeliveryCharges,
					Latitude = order.Latitude,
					Longitude = order.Longitude,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
					NoteToRider = order.NoteToRider,
					DeliveryStaffCash = order.DeliveryStaffCash,

				} : null,
				Printer = printer != null ? new
				{
					printer.Id,
					printer.Name,
					printer.Type,
					printer.IP,
					printer.Port,
					printer.DeviceID,
				} : null,
				Restaurant = order.Restaurant != null ? new
				{
					RestaurantId = order.RestaurantId,
					Name = order.Restaurant.NameAsPerTradeLicense,
					RestaurantBranchId = order.RestaurantBranchId,
					BranchName = order.RestaurantBranch != null ? order.RestaurantBranch.NameAsPerTradeLicense : "",
					Email = order.Restaurant.Email,
					BranchEmail = order.RestaurantBranch != null ? order.RestaurantBranch.Email : "",
					Logo = order.Restaurant.Logo,
					Address = order.RestaurantBranch != null ? order.RestaurantBranch.Address : "",
				} : null,
			};

			#endregion

			return result;
		}

		//Get Invoice Object
		private static object GetKitchenSlip(OrderDTO order, RestaurantPrinterSetting printer)
		{
			object result = new();

			#region result set

			result = new
			{
				OrderId = order.Id,
				OrderNo = order.OrderNo,
				CreationDateTime = order.CreationDate,
				CreationDate = order.CreationDate.Value.ToString("dd-MM-yyyy"),
				CreationTime = order.CreationDate.Value.ToString("hh:mm"),
				OrderPlacementDate = order.OrderPlacementDateTime.HasValue ? order.OrderPlacementDateTime.Value.ToString("dd-MM-yyyy") : null,
				OrderPlacementTime = order.OrderPlacementDateTime.HasValue ? order.OrderPlacementDateTime.Value.ToString("hh:mm") : null,
				FoodReadyDate = order.FoodReadyDateTime.HasValue ? order.FoodReadyDateTime.Value.ToString("dd-MM-yyyy") : null,
				FoodReadyTime = order.FoodReadyDateTime.HasValue ? order.FoodReadyDateTime.Value.ToString("hh:mm") : null,
				PickedByRiderDate = order.PickedByRiderDateTime.HasValue ? order.PickedByRiderDateTime.Value.ToString("dd-MM-yyyy") : null,
				PickedByRiderTime = order.PickedByRiderDateTime.HasValue ? order.PickedByRiderDateTime.Value.ToString("hh:mm") : null,

				Status = order.Status,
				ItemCount = order.OrderDetails.Sum(j => j.Quantity),
				DeliveryCharges = order.DeliveryCharges,
				DeliveryType = order.DeliveryType,
				Amount = order.Amount,
				VATAmount = order.TaxAmount,
				VATPercentage = order.TaxPercent,

				DiscountPercent = order.DiscountPercent,
				DiscountAmount = order.DiscountAmount,

				CouponCode = order.CouponCode,
				CouponDiscount = order.CouponDiscount,
				TotalAmount = order.TotalAmount,
				DeliveryDateTime = order.DeliveryDateTime,
				DeliveryDate = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("dd-MM-yyyy") : null,
				DeliveryTime = order.DeliveryDateTime.HasValue ? order.DeliveryDateTime.Value.ToString("hh:mm") : null,
				EstimatedDeliveryMinutes = order.EstimatedDeliveryMinutes < 0 ? 0 : order.EstimatedDeliveryMinutes,

				CashierStaffId = order.RestaurantCashierStaffId,
				Cashier = order.CashierStaff != null ? order.CashierStaff.FirstName + order.CashierStaff.LastName : null,
				RestaurantWaiterId = order.RestaurantWaiterId,
				RestaurantWaiter = order.RestaurantWaiter != null ? order.RestaurantWaiter.Name : null,
				RestaurantTableId = order.RestaurantTableId,
				Table = order.RestaurantTable != null ? order.RestaurantTable.Name : null,
				PaymentMethod = order.PaymentMethod,
				CardScheme = order.CardScheme != null ? order.CardScheme.Type : null,
				Aggregator = order.Aggregator != null ? order.Aggregator.Name : null,
				PaidTo = order.PaidTo,
				CashReceived = order.CashReceived,
				Change = order.Change,
				CardAmount = order.CardAmount,
				RedeemAmount = order.RedeemAmount,

				//IsPaid = order.IsPaid,
				PaidStatus = order.PaidStatus,
				Currency = order.Currency,
				IsCanceled = order.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled) ? true : false,
				CancelationReason = order.CancelationReason,
				PaymentRef = order.PaymentRef,
				PaymentCaptured = order.PaymentCaptured,
				IsEarningCaptured = order.IsEarningCaptured,
				OrderRef = order.OrderRef,

				Type = order.Type,
				IsAmended = order.IsAmended,
				AmendReason = order.AmendReason,
				Origin = order.Origin,
				OrderNote = order.OrderNote,

				IsPrinterAllowed = order.CashierStaff != null && order.CashierStaff.IsPrinterAllowed,
				EditCount = order.EditCount,
				IsUpdated = order.EditCount > 1 ? true : false,

				Detail = order.OrderDetails.Count > 0 ? order.OrderDetails.Select(x => new
				{
					ItemName = x.MenuItems != null ? x.MenuItems.Name : x.MenuItemName,
					Quantity = x.Quantity,
					CustomerNote = x.CustomerNote == null ? "-" : x.CustomerNote,
					UnitPrice = x.UnitPrice,
					Price = x.Price,
					TotalPrice = x.TotalPrice,

					TaxPercent = x.TaxPercent,
					TaxAmount = x.TaxAmount,
					DiscountPercent = x.DiscountPercent,
					DiscountAmount = x.DiscountAmount,

					Status = x.Status,
					IsNew = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.New) ? true : false,
					IsCanceled = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled) ? true : false,
					IsUpdated = x.IsUpdated.HasValue && x.IsUpdated.Value ? true : false,
					EditCount = x.EditCount,
					//IsUpdated = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Updated) ? true : false,
					//IsUpdated = x.EditCount > 1 ? true : false,

					ItemInfo = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.New) ? "(NEW)"
								: x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled) ? "(CXL)"
								: x.IsUpdated.HasValue && x.IsUpdated.Value ? "(UPD)"
								: "",

					/* if status equal to caneled than options are removed from printing */
					Options = x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.Canceled) ? null : x.OrderDetailOptionValues.GroupBy(x => x.MenuItemOption).Select(grp => grp.FirstOrDefault()).Select(v => new
					{
						OptionName = v.MenuItemOption,
						Values = x.OrderDetailOptionValues.Where(o => o.MenuItemOptionId == v.MenuItemOptionId).Select(i => new
						{
							Name = i.MenuItemOptionValue,
							i.UnitPrice,
							i.Quantity,
							i.Price,
							i.TotalPrice,
						}),

					}),
				}) : null,

				Customer = new
				{
					Id = order.CustomerId,
					Name = order.CustomerName,
					Email = order.CustomerEmail,
					Contact = order.CustomerContact,
					Instruction = order.NoteToRider,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
				},
				Waiter = order.RestaurantWaiter != null ? new
				{
					Id = order.RestaurantWaiter.Id,
					Name = order.RestaurantWaiter.Name,
					Email = order.RestaurantWaiter.Email,
					Contact = order.RestaurantWaiter.Contact,
					Contact2 = order.RestaurantWaiter.Contact2,
					Address = order.RestaurantWaiter.Address,
					Logo = order.RestaurantWaiter.Logo,
				} : null,
				DeliveryStaff = order.DeliveryStaff != null ? new
				{
					Name = order.DeliveryStaff.FirstName + " " + order.DeliveryStaff.LastName,
					Contact = order.DeliveryStaff.PhoneNumber,
					DeliveryCharges = order.DeliveryCharges,
					Latitude = order.Latitude,
					Longitude = order.Longitude,
					Address = order.Address,
					Street = order.Street,
					Floor = order.Floor,
					NoteToRider = order.NoteToRider,
					DeliveryStaffCash = order.DeliveryStaffCash,

				} : null,
				Printer = printer != null ? new
				{
					printer.Id,
					printer.Name,
					printer.Type,
					printer.IP,
					printer.Port,
					printer.DeviceID,
				} : null,
				Restaurant = order.Restaurant != null ? new
				{
					RestaurantId = order.RestaurantId,
					Name = order.Restaurant.NameAsPerTradeLicense,
					RestaurantBranchId = order.RestaurantBranchId,
					BranchName = order.RestaurantBranch != null ? order.RestaurantBranch.NameAsPerTradeLicense : "",
					Email = order.Restaurant.Email,
					BranchEmail = order.RestaurantBranch != null ? order.RestaurantBranch.Email : "",
					Logo = order.Restaurant.Logo,
					Address = order.RestaurantBranch != null ? order.RestaurantBranch.Address : "",

				} : null,
			};

			#endregion

			return result;
		}

		//Update Isupdate column in order details
		private async Task<bool> UpdateDetailItemsInfo(Order order)
		{
			bool result = false;

			order.OrderDetails.ToList().ForEach(x => x.IsUpdated = false);
			order.OrderDetails.Where(x => x.Status == Enum.GetName(typeof(OrderDetailStatus), OrderDetailStatus.New)).ToList().ForEach(x => x.Status = "");

			try
			{
				Order data = await _orderService.UpdateOrderAsync(order);
				return true;
			}
			catch (Exception)
			{
			}

			return result;
		}

		#endregion

	}


}
