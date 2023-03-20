using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Emails;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Order.Filter;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
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

namespace WebAPI.Controllers.Partner
{
    [Route("api/partner/orders")]
    [ApiController]
    [Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff, RestaurantCashierStaff")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IFCMUserSessionService _fCMUserSession;
        private readonly INotificationService _notificationService;
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly IRestaurantDeliveryStaffService _deliveryStaffService;
        private readonly IMessageService _messageService;
        private readonly ILogger<OrderController> _logger;
        private readonly IEmailService _emailService;

        public OrderController(IOrderService orderService
            , IMapper mapper
            , IFCMUserSessionService fCMUserSession
            , INotificationService notificationService
            , IIntegrationSettingService integrationSettingService
            , IRestaurantDeliveryStaffService deliveryStaffService
            , IMessageService messageService
            , ILogger<OrderController> logger, IEmailService emailService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _fCMUserSession = fCMUserSession;
            _notificationService = notificationService;
            _integrationSettingService = integrationSettingService;
            _deliveryStaffService = deliveryStaffService;
            _messageService = messageService;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult GetAllOrders(OrderFilter Filter)
        {
            return Ok(new SuccessResponse<IEnumerable<OrderShortDetailsDTO>>("", _orderService.GetAllByFilters(Filter)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<OrderDTO> List = _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetOrderByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPut("{OrderId}/Status/{StatusId}")]
        public async Task<IActionResult> ChangeStatus(long OrderId, OrderStatus StatusId)
        {
            IEnumerable<Order> Orders = await _orderService.GetOrderByIdAsync(OrderId);

            if (!Orders.Any())
                return Conflict(new ErrorDetails(409, "No record found. Invalid OrderId", ""));

            Order order = Orders.FirstOrDefault();
            List<OrderDetailDTO> OrderDetails = _mapper.Map<List<OrderDetailDTO>>(order.OrderDetails);

            order.Status = Enum.GetName(typeof(OrderStatus), StatusId);
            order.OrderDetails = null;

            if (order.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered) && 
                order.PaymentMethod == Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Cash))
                order.IsPaid = true;

            OrderDTO result = _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(order));
            result.OrderDetails = OrderDetails;

            if (result.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed))
            {
                result.DeliveryStaff = null;
                OrderPlacementEmailDTO emailDTO = new();
                emailDTO.Restaurant = _mapper.Map<RestaurantDTO>(order.Restaurant);
                emailDTO.Order = result;
                emailDTO.Email = order.CustomerEmail;

                await _emailService.SendOrderPlacementEmail(emailDTO);
            }

            if (result.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Delivered))
                //await SendOrderDeliveredSMS(result.CustomerContact, result.OrderNo);
                await SendOrderDeliveredEmail(result.CustomerEmail, result.CustomerName, result.Restaurant, result);

            if (result.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled))
                //await SendOrderCancellationSMS(result.CustomerContact, result.OrderNo);
                await SendOrderCancellationEmail(result.CustomerEmail, result.CustomerName, result.Restaurant, result);

            else if (result.DeliveryType == Enum.GetName(typeof(OrderType), OrderType.Pickup) && result.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady))
                await SendOrderReadyEmail(result.CustomerEmail, result.CustomerName, result.Restaurant, result);

            return Ok(new SuccessResponse<OrderDTO>("", result));
        }

        [HttpPut("{OrderId}/AssignRider/{RiderId}")]
        public async Task<IActionResult> AssignRider(long OrderId, long RiderId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.RestaurantDeliveryStaff> DeliveryStaffs = await _deliveryStaffService.GetRestaurantDeliveryStaffByIdAsync(RiderId);

            if (!DeliveryStaffs.Any())
                return Conflict(new ErrorDetails(409, "Invalid RiderId", ""));

            IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync(OrderId);

            Order order = orders.FirstOrDefault();
            order.DeliveryStaff = null;
            order.DeliveryStaffId = RiderId;
            order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.FoodReady);

            Order result = await _orderService.UpdateOrderAsync(order);

            NotificationDTO notification = new()
            {
                OriginatorId = UserId,
                OriginatorName = order.Restaurant.NameAsPerTradeLicense,
                Description = "Order Ready For Pickup",
                RecordId = order.Id,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/Restaurant/RestaurantOrder/Index",
                NotificationReceivers = new List<NotificationReceiverDTO>()
                    {
                        new NotificationReceiverDTO
                        {
                            ReceiverId = DeliveryStaffs.FirstOrDefault().UserId,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
                        }
                    },
            };
            await _notificationService.AddNotification(_mapper.Map<Notification>(notification));

            var FCMList = await _fCMUserSession.GetUserSessionTokensByUser(DeliveryStaffs.FirstOrDefault().UserId);
            if (FCMList.Any())
            {
                string[] tokens = FCMList.Select(x => x.FirebaseToken).ToArray();
                IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
                var response = PushNotifications.SendPushNotification(tokens, "Order Ready For Pickup", "", new
                {
                    order.OrderNo,
                    OrderId = order.Id
                }, settings.FirstOrDefault().DeliveryFCMKey, false);
            }

            return Ok(new SuccessResponse<RestaurantDeliveryStaffDTO>("", _mapper.Map<RestaurantDeliveryStaffDTO>(result.DeliveryStaff)));
        }

        [HttpPut("{OrderId}/NotDelivered")]
        public async Task<IActionResult> NotDelivered(long OrderId, [FromQuery] string Reason)
        {
            IEnumerable<Order> orders = await _orderService.GetOrderByIdAsync(OrderId);

            Order order = orders.FirstOrDefault();

            order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.NotDelivered);
            order.CancelationReason = Reason;

            order.OrderDetails = null;
            order.RestaurantBranch = null;
            order.Restaurant = null;
            order.Customer = null;
            order.DeliveryStaff = null;
            order.RestaurantRatings = null;

            Order result = await _orderService.UpdateOrderAsync(order);

            return Ok(new SuccessResponse<RestaurantDeliveryStaffDTO>("", _mapper.Map<RestaurantDeliveryStaffDTO>(result.DeliveryStaff)));
        }

        [HttpGet("Details/{DetailId}/Options")]
        public async Task<IActionResult> GetOrderOptions(long DetailId) => Ok(new SuccessResponse<List<OrderDetailOptionsDTO>>("", await _orderService.GetOptionsByOrderDetail(DetailId)));
        [HttpPut("{Id}/ETA/{Minutes}")]
        public async Task<IActionResult> UpdateETA(long Id, int Minutes)
        {
            IEnumerable<Order> result = await _orderService.GetOrderByIdAsync(Id);
            Order order = result.FirstOrDefault();

            order.EstimatedDeliveryMinutes = Minutes;
            order.DeliveryDateTime = DateTime.UtcNow.ToDubaiDateTime();

            return Ok(new SuccessResponse<OrderDTO>("", _mapper.Map<OrderDTO>(await _orderService.UpdateOrderAsync(order))));
        }

        private async Task<bool> SendOrderDeliveredSMS(string ContactNumber, string OrderNumber)
        {
            bool IsOTPSent = false;
            try
            {
                string Text = $"Your order {OrderNumber} has been delivered \nMDNSMGYjkzc";

                if (await _messageService.SendMessage(ContactNumber, Text))
                {
                    _logger.LogInformation("Order delivery message Sent Successfully to " + ContactNumber);

                    IsOTPSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Order delivery message Sending Failed for " + ContactNumber + " with message: " +
                                  ex.Message);
            }

            return IsOTPSent;
        }
        private async Task<bool> SendOrderCancellationSMS(string ContactNumber, string OrderNumber)
        {
            bool IsOTPSent = false;
            try
            {
                string Text = $"Your order {OrderNumber} has been cancelled \nMDNSMGYjkzc";

                if (await _messageService.SendMessage(ContactNumber, Text))
                {
                    _logger.LogInformation("Order delivery message Sent Successfully to " + ContactNumber);

                    IsOTPSent = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Order delivery message Sending Failed for " + ContactNumber + " with message: " +
                                  ex.Message);
            }

            return IsOTPSent;
        }
        private async Task SendOrderDeliveredEmail(string email, string name, RestaurantDTO restaurant, OrderDTO order)
        {
            try
            {
                await _emailService.SendGeneralEmail(new GeneralEmailDTO()
                {
                    Name = name,
                    HTMLBody = $"Your order {order.OrderNo} has been delivered",
                    Restaurant = restaurant,
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
        private async Task SendOrderCancellationEmail(string email, string name, RestaurantDTO restaurant, OrderDTO order)
        {
            try
            {
                await _emailService.SendGeneralEmail(new GeneralEmailDTO()
                {
                    Name = name,
                    HTMLBody = $"Your order {order.OrderNo} has been cancelled",
                    Restaurant = restaurant,
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
    }
}
