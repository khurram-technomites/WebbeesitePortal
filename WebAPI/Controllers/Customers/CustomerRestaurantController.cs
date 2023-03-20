using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Fatoorah.WebHook;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Restaurant.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
	[Route("api/Customer/Restaurant")]
	[ApiController]
	public class CustomerRestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;
		private readonly IRestaurantRatingService _restaurantRatingService;
		private readonly IRestaurantBranchService _restaurantBranchService;
		private readonly ICouponService _couponService;
		private readonly IMenuItemService _menuItemService;
		private readonly IMenuItemOptionService _menuItemOptionService;
		private readonly UserManager<AppUser> _userManager;
		private readonly ICustomerService _customerService;
		private readonly IMapper _mapper;
		private readonly INumberRangeService _numberRangeService;
		private readonly IOrderService _orderService;
		private readonly IFatoorahService _fatoorahService;
		private readonly ICustomerTransactionHistoryService _historyService;
		private readonly IFCMUserSessionService _fCMUserSession;
		private readonly ICouponRedemptionService _couponRedemptionService;
		private readonly INotificationService _notificationService;
		private readonly IIntegrationSettingService _integrationSettingService;
		private readonly IConfiguration _config;

		public CustomerRestaurantController(IRestaurantRatingService restaurantRatingService, IMapper mapper, IRestaurantService restaurantService,
			IRestaurantBranchService restaurantBranchService, ICouponService couponService, UserManager<AppUser> userManager, ICustomerService customerService,
			IMenuItemService menuItemService, IMenuItemOptionService menuItemOptionService, INumberRangeService numberRangeService, IOrderService orderService,
			IFatoorahService fatoorahService, ICustomerTransactionHistoryService historyService, IFCMUserSessionService fCMUserSession, ICouponRedemptionService couponRedemptionService,
			INotificationService notificationService, IIntegrationSettingService integrationSettingService, IConfiguration config)
		{
			_restaurantService = restaurantService;
			_restaurantRatingService = restaurantRatingService;
			_restaurantBranchService = restaurantBranchService;
			_couponService = couponService;
			_customerService = customerService;
			_userManager = userManager;
			_mapper = mapper;
			_menuItemService = menuItemService;
			_menuItemOptionService = menuItemOptionService;
			_numberRangeService = numberRangeService;
			_orderService = orderService;
			_fatoorahService = fatoorahService;
			_historyService = historyService;
			_fCMUserSession = fCMUserSession;
			_couponRedemptionService = couponRedemptionService;
			_notificationService = notificationService;
			_integrationSettingService = integrationSettingService;
			_config = config;
		}

		[HttpPost("GetAll")]
		public IActionResult GetAllRestaurantsNearMe(RestaurantFilter Filter) => Ok(new SuccessResponse<IEnumerable<RestaurantCardResponseDTO>>("", _mapper.Map<IEnumerable<RestaurantCardResponseDTO>>(_restaurantService.GetAllRestaurantsNearMe(Filter))));

		[HttpPost("Trending")]
		public IActionResult Trending(RestaurantFilter Filter) => Ok(new SuccessResponse<IEnumerable<RestaurantCardResponseDTO>>("", _restaurantService.GetTrending(Filter)));

		[HttpGet("{RestaurantId}/GetActiveBranches")]
		public async Task<IActionResult> GetAllACtiveRestaurants(long RestaurantId) => Ok(new SuccessResponse<IEnumerable<RestaurantBranchDTO>>("", _mapper.Map<IEnumerable<RestaurantBranchDTO>>(await _restaurantBranchService.GetAllActiveBranchesByRestaurant(RestaurantId))));

		[HttpGet("{slug}")]
		public async Task<IActionResult> GetBySlug(string slug, double? lat, double? lng)
		{
			List<RestaurantBranchDTO> list = _mapper.Map<List<RestaurantBranchDTO>>(await _restaurantBranchService.GetRestaurantBranchesBySlug(slug));

			if (!list.Any())
				return Conflict(new ErrorDetails(409, "No record found!", ""));

			if (!lat.HasValue && !lng.HasValue)
				list.ForEach(x => x.Distance = null);
			else
				list.ForEach(x => x.Distance = DistanceHelper.DistanceTo((double)lat, (double)lng, (double)x.Latitude, (double)x.Longitude).ToString("n2") + " Km");

			list.ForEach(x => x.Restaurant.restaurantRatings.RemoveAll(x => x.ShowOnWebsite == false));

			return Ok(new SuccessResponse<IEnumerable<RestaurantBranchDTO>>("", list));
		}

		[HttpPost("Rating")]
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> PostReview(RestaurantRatingDTO Model)
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			Model.UserId = UserId;
			Model.Status = Enum.GetName(typeof(Status), Status.Processing);
			return Ok(new SuccessResponse<RestaurantRatingDTO>("", _mapper.Map<RestaurantRatingDTO>(await _restaurantRatingService.AddRestaurantRatingAsync(_mapper.Map<RestaurantRating>(Model)))));
		}
		[HttpGet("Branch/{BranchId}/PopularCategories")]
		public async Task<IActionResult> GetPopularCategoriesByBranch(long BranchId) => Ok(new SuccessResponse<IEnumerable<PopularCategoriesDTO>>("", _mapper.Map<IEnumerable<PopularCategoriesDTO>>(await _restaurantService.GetPopularCategoriesByBranch(BranchId))));

		[HttpGet]
		public IActionResult GetByOrigin()
		{
			string origin = Request.Headers["origin"];

			if (string.IsNullOrEmpty(origin))
				return Conflict(new ErrorDetails(409, "Invalid origin", ""));

			IEnumerable<LandingPageResponseDTO> resultList = _restaurantService.GetRestaurantDetailsByOrigin(origin);

			return Ok(new SuccessResponse<LandingPageResponseDTO>("", resultList.FirstOrDefault()));
		}

		[HttpGet("NearestBranch/{lat}/{lng}")]
		public async Task<IActionResult> GetNearestBranches(decimal lat, decimal lng)
		{
			string origin = Request.Headers["origin"];

			if (string.IsNullOrEmpty(origin))
				return Conflict(new ErrorDetails(409, "Invalid origin", ""));

			IEnumerable<Models.Restaurant> restaurants = await _restaurantService.GetRestaurantByOrigin(origin);

			if (!restaurants.Any())
				return Conflict(new ErrorDetails(409, "Invalid UniqueKey, No record found", ""));

			var Branch = _restaurantService.GetNearestRestaurant(restaurants.FirstOrDefault().Id, lat, lng);

			if (Branch is null)
				return Conflict(new ErrorDetails(409, "Sorry, This restaurant does not deliver to this location", ""));
			else
				return Ok(new SuccessResponse<object>("", Branch));
		}

		[HttpGet("Branch/{BranchId}/Details")]
		public async Task<IActionResult> GetBranchDetails(long BranchId)
		{
			long? customerId = null;
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!string.IsNullOrEmpty(UserId))
			{
				IEnumerable<Models.Customer> Customer = await _customerService.GetByUserIdAsync(UserId);
				customerId = Customer.FirstOrDefault().Id;
			}

			return Ok(new SuccessResponse<IEnumerable<LandingPageResponseDTO>>("", await _restaurantService.GetRestaurantBranchDetails(BranchId, customerId)));
		}

		[HttpGet("Branch/{BranchId}/Menu")]
		public async Task<IActionResult> Menu(long BranchId) => Ok(new SuccessResponse<List<BranchMenuDTO>>("", _mapper.Map<List<BranchMenuDTO>>(await _restaurantService.GetBranchMenu(BranchId))));

		[HttpPost("ValidateCoupon")]
		public async Task<IActionResult> ValidateCoupon(CouponValidationDTO Model)
		{
			string origin = Request.Headers["origin"];

			if (string.IsNullOrEmpty(origin))
				return Conflict(new ErrorDetails(409, "Invalid origin", ""));

			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(Model.PhoneNumber))
				return Conflict(new ErrorDetails(409, "Please login or Enter a valid mobile number", null));

			IEnumerable<Models.Restaurant> restaurants = await _restaurantService.GetRestaurantByOrigin(origin);
			IEnumerable<Coupon> coupons = await _couponService.GetByCodeAsync(Model.CouponCode);

			if (!coupons.Any())
				return Conflict(new ErrorDetails(409, "Invalid coupon", null));

			Coupon coupon = coupons.FirstOrDefault();

			if (coupon.RestaurantId != null && !restaurants.Any())
				return Conflict(new ErrorDetails(409, "Invalid coupon", null));

			if (coupon.RestaurantId != null && coupon.RestaurantId != restaurants.FirstOrDefault().Id)
				return Conflict(new ErrorDetails(409, "Invalid coupon", null));

			if (coupon.Expiry < DateTime.UtcNow)
				return Conflict(new ErrorDetails(409, "Coupon has been expired", null));

			if (coupon.IsOpenToAll && coupon.CouponRedemptions.Count < coupon.MaxUsage)
			{
				if (coupon.Frequency.HasValue && coupon.Frequency.Value <= coupon.CouponRedemptions.Where(x => x.PhoneNumber == Model.PhoneNumber).Count())
					return Conflict(new ErrorDetails(409, "Invalid coupon", null));

				return Ok(new SuccessResponse<CouponDTO>("Coupon is valid", _mapper.Map<CouponDTO>(coupon)));
			}
			else if (coupon.IsOpenToAll && coupon.CouponRedemptions.Count >= coupon.MaxUsage)
				return Conflict(new ErrorDetails(409, "This coupon is already availed", null));

			if (!coupon.IsOpenToAll && !string.IsNullOrEmpty(UserId))
			{
				IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);
				Customer customer = customers.FirstOrDefault();

				if (coupon.CustomerCoupons.Where(x => x.CustomerId == customer.Id).Any())
				{
					if (coupon.Frequency.HasValue && coupon.Frequency.Value > coupon.CouponRedemptions.Where(x => x.UserId == UserId).Count())
						return Ok(new SuccessResponse<CouponDTO>("Coupon is valid", _mapper.Map<CouponDTO>(coupon)));
				}
			}

			return Conflict(new ErrorDetails(409, "Invalid coupon", null));
		}

		[HttpGet("Branch/{BranchId}/ValidateDistanct/{lat}/{lng}")]
		public async Task<IActionResult> ValidateDistance(long BranchId, decimal lat, decimal lng)
		{
			IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(BranchId);

			RestaurantBranch branch = branches.FirstOrDefault();

			double dist = DistanceHelper.DistanceTo((double)branch.Latitude, (double)branch.Longitude, (double)lat, (double)lng);

			if (dist > (double)branch.ServiceDistance)
				return Conflict(new ErrorDetails(409, "Sorry for the inconvenience. We are currently not delivering to your area. Kindly select another address.", null));

			double DeliveryCharges = 0;
			if (branch.DeliveryType == Enum.GetName(typeof(DeliveryType), DeliveryType.PerKilometer))
			{
				DeliveryCharges = Math.Round(dist * (double)branch.DeliveryCharges, 2);
			}
			else
			{
				DeliveryCharges = Math.Round((double)branch.DeliveryCharges, 2);
			}

			return Ok(new SuccessResponse<object>("Delivery is available", new
			{
				BranchId,
				branch.Address,
				branch.Latitude,
				branch.Longitude,
				DeliveryCharges,
				branch.IsClose,
				branch.ClosingTimeSpan,
				MinOrderPrice = 60M
			}));
		}

		[HttpPost("PlaceOrder")]
		public async Task<IActionResult> PlaceOrder(OrderPlacementDTO orderDTO, [FromQuery] Client Client = 0)
		{
			List<NotificationReceiver> notificationReceivers = new();
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			long CouponId = 0;
			decimal DiscountAmount = 0, distance = 0;
			IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(orderDTO.RestaurantBranchId);

			if (branches.FirstOrDefault().IsClose)
				return Conflict(new ErrorDetails(409, "Restaurant is closed.", null));

			Order order = new();

			//Mapping OrderPlacementDTO to Order Entity
			//Maps all 3 levels, Order, OrderDetails, OrderDetailsOptionValues
			//See mapper for more
			orderDTO.Origin = Enum.GetName(typeof(OrderOrigin), OrderOrigin.Customer);
			order = _mapper.Map<Order>(orderDTO);
			order.OrderNo = await _numberRangeService.GetNumberRangeByName("ORDER");
			order.TaxPercent = branches.FirstOrDefault().Restaurant.TaxPercent;
			order.TaxAmount = 0;
			order.DiscountAmount = 0;
			order.DiscountPercent = 0;
			order.RedeemAmount = 0;
			order.Status = Enum.GetName(typeof(Status), Status.Pending);
			order.Currency = "AED";
			order.OrderPlacementDateTime = DateTime.UtcNow.ToDubaiDateTime();
			order.RestaurantId = branches.FirstOrDefault().RestaurantId;
			order.EstimatedDeliveryMinutes = branches.FirstOrDefault().DeliveryMinutes;
			order.DeliveryDateTime = DateTime.UtcNow.ToDubaiDateTime();

			//Do not remove this
			order.CustomerName = order.CustomerName.Replace("null", "");

			if (!order.CustomerContact.StartsWith("+"))
			{
				order.CustomerContact = "+" + order.CustomerContact;
			}

			if (!string.IsNullOrEmpty(UserId))
			{
				IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);

				if (customers.Any())
					order.CustomerId = customers.FirstOrDefault().Id;
			}

			//If option value is main, map it directly in menuitem
			foreach (var Detail in order.OrderDetails)
			{
				Detail.Price += Detail.OrderDetailOptionValues.Sum(x => x.TotalPrice);
				Detail.TotalPrice = Detail.Quantity * Detail.Price;
			}

			order.Status = Enum.GetName(typeof(Status), Status.Pending);
			order.Amount = order.OrderDetails.Sum(x => x.TotalPrice);

			if (order.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Delivery))
			{
				distance = (decimal)DistanceHelper.DistanceTo((double)branches.FirstOrDefault().Latitude, (double)branches.FirstOrDefault().Longitude, (double)orderDTO.Latitude, (double)orderDTO.Longitude);

				if (branches.FirstOrDefault().ServiceDistance < distance && order.DeliveryType == Enum.GetName(typeof(DeliveryType), DeliveryType.Delivery))
					return Conflict(new ErrorDetails(409, "Sorry for the inconvenience. We are currently not delivering to your area. Kindly select another address.", null));

				if (branches.FirstOrDefault().DeliveryType == Enum.GetName(typeof(DeliveryType), DeliveryType.Fixed))
					order.DeliveryCharges = branches.FirstOrDefault().DeliveryCharges;
				else
				{
					order.DeliveryCharges = branches.FirstOrDefault().DeliveryCharges * distance;
				}
			}
			else
				order.DeliveryCharges = 0M;

			if (!string.IsNullOrEmpty(orderDTO.CouponCode))
			{
				IEnumerable<Coupon> coupons = await _couponService.GetByCodeAsync(orderDTO.CouponCode);

				if (coupons.Any())
				{
					Coupon coupon = coupons.FirstOrDefault();
					CouponId = coupon.Id;
					order.CouponCode = coupon.CouponCode;

					if (coupon.Type == Enum.GetName(typeof(DiscountType), DiscountType.Percentage))
					{
						DiscountAmount = order.Amount * coupon.Value.Value / 100;

						//if discount amount is greater than Maximum limit, set discount to maximum
						//This is important
						if (coupon.MaxAmount.HasValue && DiscountAmount > coupon.MaxAmount.Value)
							DiscountAmount = coupon.MaxAmount.Value;

						order.CouponDiscount = DiscountAmount;
						order.DiscountAmount = DiscountAmount;

					}
					else if (coupon.Type == Enum.GetName(typeof(DiscountType), DiscountType.FixedAmount))
					{
						if (order.Amount < coupon.Value.Value)
							order.CouponDiscount = order.Amount;
						else
							order.CouponDiscount = coupon.Value.Value;
					}


					order.DiscountPercent = coupon.Value.Value;
				}
			}

			//Tax calculation
			order.TaxAmount = ((order.Amount - order.CouponDiscount + order.DeliveryCharges) * order.TaxPercent) / 100;

			//Total calculation
			order.TotalAmount = order.Amount - order.CouponDiscount + order.DeliveryCharges + order.TaxAmount;

			var ActiveBranch = branches.FirstOrDefault();

			if (ActiveBranch != null && order.TotalAmount < ActiveBranch.MinOrderPrice)
			{
				return Conflict(new ErrorDetails(409, string.Format("You must have a minimum order amount of {0} to place your order. Your current order total is {1}", ActiveBranch.MinOrderPrice, order.Amount), null));
			}

			OrderDTO dTO = _mapper.Map<OrderDTO>(await _orderService.AddOrderAsync(order));

			if (order.CouponDiscount != 0)
			{
				CouponRedemption couponRedemption = new();
				couponRedemption.UserId = UserId ?? null;
				couponRedemption.CouponId = CouponId;
				couponRedemption.OrderId = dTO.Id;
				couponRedemption.PhoneNumber = dTO.CustomerContact;

				await _couponRedemptionService.AddCouponRedemption(couponRedemption);
			}

			if (orderDTO.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Cash) && branches.FirstOrDefault().RestaurantServiceStaffs.Any())
			{
				foreach (var staff in branches.FirstOrDefault().RestaurantServiceStaffs)
				{
					notificationReceivers.Add(new NotificationReceiver
					{
						ReceiverId = staff.UserId,
						IsSeen = false,
						IsDelivered = false,
						IsRead = false,
						ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
					});
				}
				notificationReceivers.Add(new NotificationReceiver
				{
					ReceiverId = branches.FirstOrDefault().Restaurant.UserId,
					IsSeen = false,
					IsDelivered = false,
					IsRead = false,
					ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
				});

				Notification notification = new()
				{
					OriginatorId = UserId,
					OriginatorName = dTO.CustomerName,
					Description = "New Order Placed",
					RecordId = dTO.Id,
					OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
					Url = "/Restaurant/RestaurantOrder/Index",
					NotificationReceivers = notificationReceivers
				};

				await _notificationService.AddNotification(notification);

				List<FCMUserSession> FCMList = new();
				foreach (var staff in branches.FirstOrDefault().RestaurantServiceStaffs)
				{
					IEnumerable<FCMUserSession> List = await _fCMUserSession.GetUserSessionTokensByUser(staff.UserId);
					FCMList.AddRange(List);
				}

				if (FCMList.Any())
				{
					string[] tokens = FCMList.Select(x => x.FirebaseToken).ToArray();
					IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
					var response = PushNotifications.SendPushNotification(tokens, string.Format("Hey! New {0} Order ({1}) received", dTO.DeliveryType, dTO.OrderNo), "", new
					{
						dTO.OrderNo,
						OrderId = dTO.Id
					}, settings.FirstOrDefault().PartnerFCMKey, false);
				}
			}

			if (orderDTO.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card))
			{
				var branch = branches.FirstOrDefault();

				string SupplierCode = "";
				string RestaurantPath = "https://fougito.com/";

				if (branch != null)
				{
					IEnumerable<WebAPI.Models.Restaurant> Restaurants = await _restaurantService.GetRestaurantByIdAsync(branch.RestaurantId);

					var restaurant = Restaurants.FirstOrDefault();
					if (restaurant != null)
					{
						SupplierCode = restaurant.SupplierCode;

						if (Enum.GetName(typeof(Client), Client) == Enum.GetName(typeof(Client), Client.Website))
							RestaurantPath = string.Format("{0}/payment-success/{1}", restaurant.Origin, dTO.Id);
						else if (Enum.GetName(typeof(Client), Client) == Enum.GetName(typeof(Client), Client.Mobile))
							RestaurantPath = string.Format("{0}/WebView/Index?OrderId={1}", _config.GetValue<string>("WebAppURL"), dTO.Id);
					}
				}

				return Ok(new SuccessResponse<string>("", await _fatoorahService.InitiatePayment(dTO, RestaurantPath, SupplierCode)));
			}

			return Ok(new SuccessResponse<OrderDTO>("", dTO));
		}

		[HttpGet("Order/{OrderId}/Repay")]
		public async Task<IActionResult> Repay(long OrderId, [FromQuery] Client Client = 0)
		{
			IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync(OrderId);
			Order Order = orders.FirstOrDefault();
			IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(Order.RestaurantBranchId);

			if (Order.IsPaid)
			{
				return Ok(new SuccessResponse<string>("Order payment is already done.", ""));
			}

			if (!Order.IsPaid && Order.PaymentCaptured)
			{
				return Ok(new SuccessResponse<string>("Order payment is in process.", ""));
			}

			if (Order.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card))
			{
				var branch = branches.FirstOrDefault();

				string SupplierCode = "";
				string RestaurantPath = "https://fougito.com/";

				if (branch != null)
				{
					IEnumerable<WebAPI.Models.Restaurant> Restaurants = await _restaurantService.GetRestaurantByIdAsync(branch.RestaurantId);

					var restaurant = Restaurants.FirstOrDefault();
					if (restaurant != null)
					{
						SupplierCode = restaurant.SupplierCode;

						if (Enum.GetName(typeof(Client), Client) == Enum.GetName(typeof(Client), Client.Website))
							RestaurantPath = string.Format("{0}/payment-success/{1}", restaurant.Origin, Order.Id);
						else if (Enum.GetName(typeof(Client), Client) == Enum.GetName(typeof(Client), Client.Mobile))
							RestaurantPath = string.Format("{0}/WebView/Index?OrderId={1}", _config.GetValue<string>("WebAppURL"), Order.Id);
					}
				}

				return Ok(new SuccessResponse<string>("", await _fatoorahService.InitiatePayment(_mapper.Map<OrderDTO>(Order), RestaurantPath, SupplierCode)));
			}

			return Ok(new SuccessResponse<string>("Your card is already captured", ""));
		}

		[HttpGet("Paid/{OrderId}")]
		public async Task<IActionResult> Paid(long OrderId, [FromQuery(Name = "PaymentId")] string PaymentId)
		{
			string origin = Request.Headers["origin"];

			List<NotificationReceiver> notificationReceivers = new();
			PaymentInquiryResponseDTO PaymentResponse = await _fatoorahService.GetPaymentResponse(PaymentId);
			CustomerTransactionHistory transactionHistory = new();

			if (PaymentResponse.IsSuccess)
			{
				IEnumerable<Order> Orders = await _orderService.GetOrderByIdAsync(OrderId);
				Order order = Orders.FirstOrDefault();
				order.OrderDetails = null;
				IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(order.RestaurantBranchId);

				if (order.IsPaid)
				{
					return Conflict(new ErrorDetails(409, "Order is already paid!", ""));
				}

				PaymentInquiryDataDTO Payment = PaymentResponse.Data;

				if ((Payment != null && Payment.InvoiceStatus == "Paid"))
				{
					InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Succss").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

					transactionHistory.NameOnCard = Payment.CustomerName;
					transactionHistory.MaskCardNo = InvoiceTransaction.CardNumber;
					transactionHistory.TransactionStatus = InvoiceTransaction.TransactionStatus;
					transactionHistory.Amount = (decimal)Payment.InvoiceValue;
					transactionHistory.OrderId = order.Id;
					transactionHistory.PaymentId = PaymentId;
					transactionHistory.Origin = string.IsNullOrEmpty(origin) ? null : origin;

					if (order.CustomerId.HasValue)
					{
						transactionHistory.CustomerId = order.CustomerId.Value;
					}

					await _historyService.AddTransactionAsync(transactionHistory);

					order.IsPaid = true;
					order.PaymentCaptured = true;
					order.InvoiceRef = PaymentId;

					await _orderService.UpdateOrderAsync(order);


					List<RestaurantServiceStaff> servicestaffs = order.Restaurant.RestaurantServiceStaffs.Where(x => x.RestaurantBranchId == order.RestaurantBranchId).ToList();
					List<FCMUserSession> FCMList = new();
					foreach (var staff in servicestaffs)
					{
						IEnumerable<FCMUserSession> List = await _fCMUserSession.GetUserSessionTokensByUser(staff.UserId);
						FCMList.AddRange(List);
					}

					if (FCMList.Any())
					{
						string[] tokens = FCMList.Select(x => x.FirebaseToken).ToArray();
						IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
						var response = PushNotifications.SendPushNotification(tokens, string.Format("Hey! New {0} Order ({1}) received", order.DeliveryType, order.OrderNo), "", new
						{
							order.OrderNo,
							OrderId = order.Id
						}, settings.FirstOrDefault().PartnerFCMKey, false);
					}

					if (order.Restaurant.RestaurantServiceStaffs.Where(x => x.RestaurantBranchId == order.RestaurantBranchId).Any())
					{
						foreach (var staff in branches.FirstOrDefault().RestaurantServiceStaffs)
						{
							notificationReceivers.Add(new NotificationReceiver
							{
								ReceiverId = staff.UserId,
								IsSeen = false,
								IsDelivered = false,
								IsRead = false,
								ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
							});
						}
						notificationReceivers.Add(new NotificationReceiver
						{
							ReceiverId = branches.FirstOrDefault().Restaurant.UserId,
							IsSeen = false,
							IsDelivered = false,
							IsRead = false,
							ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
						});

						Notification notification = new()
						{
							OriginatorId = "",
							OriginatorName = order.CustomerName,
							Description = "New Order Placed",
							RecordId = order.Id,
							OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
							Url = "/Restaurant/RestaurantOrder/Index",
							NotificationReceivers = notificationReceivers
						};

						await _notificationService.AddNotification(notification);
					}

					return Ok(new SuccessResponse<object>("Payment successful", new
					{
						paymentStatus = "Paid",
						orderNo = order.OrderNo,
						totalAmount = order.TotalAmount,
						orderId = order.Id
					}));
				}
				else if (Payment != null && Payment.InvoiceStatus == "Pending")
				{
					InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Failed").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

					if (InvoiceTransaction != null)
					{
						transactionHistory.NameOnCard = Payment.CustomerName;
						transactionHistory.MaskCardNo = InvoiceTransaction.CardNumber;
						transactionHistory.TransactionStatus = InvoiceTransaction.TransactionStatus;
						transactionHistory.Amount = (decimal)Payment.InvoiceValue;
						transactionHistory.OrderId = order.Id;
						transactionHistory.PaymentId = PaymentId;
						transactionHistory.Origin = string.IsNullOrEmpty(origin) ? null : origin;

						if (order.CustomerId.HasValue)
						{
							transactionHistory.CustomerId = order.CustomerId.Value;
						}

						await _historyService.AddTransactionAsync(transactionHistory);

						order.IsPaid = false;
						order.InvoiceRef = PaymentId;
						order.PaymentCaptured = false;
						//cancel order
						order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
						//order.Restaurant = null;
						//order.RestaurantBranch = null;

						await _orderService.UpdateOrderAsync(order);

						return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
						{
							paymentStatus = "Failed",
							orderNo = order.OrderNo,
							totalAmount = order.TotalAmount,
							orderId = order.Id
						}));
					}
					else
					{
						order.IsPaid = false;
						order.InvoiceRef = PaymentId;
						order.PaymentCaptured = true;
						order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);

						await _orderService.UpdateOrderAsync(order);

						return Ok(new SuccessResponse<object>("Payment is in process", new
						{
							paymentStatus = "Pending",
							orderNo = order.OrderNo,
							totalAmount = order.TotalAmount,
							orderId = order.Id
						}));
					}
				}
				else
				{
					order.IsPaid = false;
					order.InvoiceRef = PaymentId;
					order.PaymentCaptured = false;
					order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
					await _orderService.UpdateOrderAsync(order);

					return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
					{
						paymentStatus = "Failed",
						orderNo = order.OrderNo,
						totalAmount = order.TotalAmount,
						orderId = order.Id
					}));


				}
			}
			return Conflict(new ErrorDetails(409, "Payment failed", ""));
		}

		[HttpPost("WebHook")]
		public async Task<IActionResult> WebHook([FromHeader(Name = "MyFatoorah-Signature")] string signatureHeader)
		{
			try
			{
				var json = new StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
				string headerSignature = "";
				bool isValidSignature = true;
				if (signatureHeader != null)
				{
					isValidSignature = false;
					headerSignature = signatureHeader.ToString();
				}
				var model = JsonConvert.DeserializeObject<WebHookResponseDTO<object>>(json);
				switch (model.EventType)
				{
					case (int)WebhookEvents.TransactionsStatusChanged:
						var transactionModel = JsonConvert.DeserializeObject<WebHookResponseDTO<WebHookTransactionStatusDTO>>(json);
						if (!isValidSignature)
						{
							isValidSignature = _fatoorahService.ValidateSignature(transactionModel, headerSignature);
							if (!isValidSignature) return BadRequest("Invalid Signature");
						}
						else
							return await Paid(Convert.ToInt64(transactionModel.Data.UserDefinedField), transactionModel.Data.PaymentId);
						break;
					case (int)WebhookEvents.RefundStatusChanged:
						var refundModel = JsonConvert.DeserializeObject<WebHookResponseDTO<WebHookRefundDTO>>(json);
						if (!isValidSignature)
						{
							isValidSignature = _fatoorahService.ValidateSignature(refundModel, headerSignature);
							if (!isValidSignature) return BadRequest("Invalid Signature");
						}
						break;
					case (int)WebhookEvents.BalanceTransferred:
						var depositModel = JsonConvert.DeserializeObject<WebHookResponseDTO<WebHookDepositeDTO>>(json);
						if (!isValidSignature)
						{
							isValidSignature = _fatoorahService.ValidateSignature(depositModel, headerSignature);
							if (!isValidSignature) return BadRequest("Invalid Signature");
						}
						break;
					case (int)WebhookEvents.SupplierStatusChanged:
						var supplierModel = JsonConvert.DeserializeObject<WebHookResponseDTO<WebHookSupplierStatusDTO>>(json);
						if (!isValidSignature)
						{
							isValidSignature = _fatoorahService.ValidateSignature(supplierModel, headerSignature);
							if (!isValidSignature) return BadRequest("Invalid Signature");
						}
						//Do Some Work
						break;
				}
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}
	}
}
