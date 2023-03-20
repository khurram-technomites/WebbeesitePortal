using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Emails;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier/Order")]
    [ApiController]
    [Authorize]
    public class SupplierOrderController : ControllerBase
    {
        private readonly ISupplierOrderService _supplierOrder;
        private readonly ISupplierService _supplierService;
        private readonly ISupplierCouponService _supplierCouponService;
        private readonly ISupplierCouponRedemptionService _supplierCouponRedemptionService;
        private readonly INumberRangeService _numberRangeService;
        private readonly IRestaurantService _restaurantService;
        private readonly INotificationService _notificationService;
        private readonly IFatoorahService _fatoorahService;
        private readonly IRestaurantTransactionHistoryService _historyService;
        private readonly IMapper _mapper;

        public SupplierOrderController(ISupplierOrderService supplierOrder,
            ISupplierCouponService supplierCouponService,
            IMapper mapper,
            INumberRangeService numberRangeService,
            IRestaurantService restaurantService,
            INotificationService notificationService,
            ISupplierCouponRedemptionService supplierCouponRedemptionService,
            ISupplierService supplierService,
            IFatoorahService fatoorahService, IRestaurantTransactionHistoryService historyService)
        {
            _supplierOrder = supplierOrder;
            _supplierCouponService = supplierCouponService;
            _supplierCouponRedemptionService = supplierCouponRedemptionService;
            _numberRangeService = numberRangeService;
            _restaurantService = restaurantService;
            _notificationService = notificationService;
            _supplierService = supplierService;
            _mapper = mapper;
            _fatoorahService = fatoorahService;
            _historyService = historyService;
        }

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(SupplierOrderPlacementDTO supplierOrder)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Models.Restaurant> restaurant = await _restaurantService.GetRestaurantByUserAsync(UserId);
            IEnumerable<Models.Supplier> SupplierList = await _supplierService.GetByIdAsync(supplierOrder.SupplierId);

            SupplierOrder order = _mapper.Map<SupplierOrder>(supplierOrder);
            order.OrderNo = await _numberRangeService.GetNumberRangeByName("SUPPLIERORDER");
            order.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Pending);
            order.IsPaid = false;
            order.Currency = "AED";
            order.RestaurantId = restaurant.FirstOrDefault().Id;
            order.RestaurantName = restaurant.FirstOrDefault().NameAsPerTradeLicense;
            order.RestaurantEmail = restaurant.FirstOrDefault().Email;
            order.RestauantContact = !string.IsNullOrEmpty(supplierOrder.Contact) ? supplierOrder.Contact : restaurant.FirstOrDefault().PhoneNumber;
            //order.RestaurantStreetAddress = !string.IsNullOrEmpty(supplierOrder.StreetAddress) ? supplierOrder.StreetAddress : "-";

            order.PaymentMethod = Enum.GetName(typeof(HelperClasses.Classes.PaymentMethod), HelperClasses.Classes.PaymentMethod.Card);
            order.TaxAmount = 0;
            order.TaxPercent = 0;
            order.Amount = order.SupplierOrderDetails.Sum(x => x.TotalPrice);

            if (supplierOrder.SupplierCouponId != 0) {
                long supplierCouponId = (long)supplierOrder.SupplierCouponId;
                IEnumerable<SupplierCoupon> coupons = await _supplierCouponService.GetByIdAsync(supplierCouponId);
                SupplierCoupon coupon = coupons.FirstOrDefault();

                order.CouponCode = coupon.CouponCode;
                order.CouponDiscount = supplierOrder.SupplierCouponDiscountAmount;
                order.TotalAmount = order.Amount - supplierOrder.SupplierCouponDiscountAmount;

            }
            else
            {
                order.TotalAmount = order.Amount;

            }
            
            SupplierOrder orderResult = await _supplierOrder.AddSupplierOrderAsync(order);

            if (supplierOrder.SupplierCouponId != 0)
            {
                SupplierCouponRedemption newRedeem = new SupplierCouponRedemption();
                newRedeem.SupplierCouponId = (long)supplierOrder.SupplierCouponId;
                newRedeem.UserId = UserId;
                newRedeem.PhoneNumber = restaurant.FirstOrDefault().PhoneNumber;
                newRedeem.SupplierOrderId = orderResult.Id;

                await _supplierCouponRedemptionService.AddSupplierCouponRedemption(newRedeem);
            }


            Notification notification = new()
            {
                OriginatorId = UserId,
                OriginatorName = restaurant.FirstOrDefault().NameAsPerTradeLicense,
                Description = "New Order Placed",
                RecordId = orderResult.Id,
                OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                Url = "/Restaurant/RestaurantOrder/Index",
                NotificationReceivers = new List<NotificationReceiver>()
                {
                    new NotificationReceiver
                    {
                        ReceiverId = SupplierList.FirstOrDefault().UserId,
                        IsSeen = false,
                        IsDelivered = false,
                        IsRead = false,
                        ReceiverType = Enum.GetName(typeof(Logins), Logins.Supplier),
                    }
                }
            };

            await _notificationService.AddNotification(notification);

            string SupplierCode = "";
            string RestaurantPath = "https://fougito.com/";

            Models.Supplier supplier = SupplierList.FirstOrDefault();
            if (supplier != null)
            {
                SupplierCode = supplier.SupplierCode;
                RestaurantPath = string.Format("{0}/Restaurant/Shop/Paid/{1}", "https://portal.fougitodemo.com", orderResult.Id);
            }

            return Ok(await _fatoorahService.InitiatePayment(orderResult, RestaurantPath, SupplierCode));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllAsync()));
        }

        [HttpGet("Restaurants/{RestaurantId}")]
        public async Task<IActionResult> GetAllByRestaurantId(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllByRestaurantAsync(restaurantId)));
        }

        [HttpPost("GetByStatus")]
        public async Task<IActionResult> GetAllByBranchAndStatus(SupplierOrderDTO orders)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllOrdersBySupplierAndStatus(orders.SupplierId , orders.Status)));
        }
        [HttpPost("GetByStatusandDate")]
        public async Task<IActionResult> GetAllByDateAndStatus(SupplierOrderDTO orders)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllOrdersByDateAndStatus( orders.SupplierId, orders.Status , orders.OrderRequiredDate , orders.OrderRequiredDate2)));
        }
        [HttpPost("GetByStatus/Restaurants")]
        public async Task<IActionResult> GetAllRestaurantOrdersByStatus(SupplierOrderDTO orders)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllRestaurantOrdersByStatus(orders.RestaurantId, orders.Status)));
        }

        [HttpGet("GetBySupplierId/{supplierId}")]
        public async Task<IActionResult> GetBySupplierIdAsync(long supplierId)
        {
            IEnumerable<SupplierOrderDTO> orders = _mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetAllBySupplierId(supplierId));
            return Ok(orders);
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            IEnumerable<SupplierOrderDTO> orders = _mapper.Map<IEnumerable<SupplierOrderDTO>>(await _supplierOrder.GetByIdAsync(Id));
            return Ok(orders);
        }
        [HttpPut("ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(SupplierOrderDTO orderModel)
        {
            IEnumerable<SupplierOrder> orderList = await _supplierOrder.GetByIdAsync(orderModel.Id);
            SupplierOrder order = orderList.FirstOrDefault();
            order.Status = orderModel.Status;

            SupplierOrderDTO model = _mapper.Map<SupplierOrderDTO>(await _supplierOrder.UpdateSupplierOrderAsync(order));

            //if (model.Status == Enum.GetName(typeof(OrderStatus), OrderStatus.Confirmed))
            //{
            //    SupplierOrderPlacementEmailDTO emailDTO = new();
            //    emailDTO.Restaurant = _mapper.Map<RestaurantDTO>(order.Restaurant);
            //    emailDTO.Order = model;
            //    emailDTO.Email = order.RestaurantEmail;

            //    //await _emailService.SendOrderPlacementEmail(emailDTO);
            //}

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet("Paid/{OrderId}/{PaymentId}")]
        public async Task<IActionResult> Paid(long OrderId, string PaymentId)
        {
            PaymentInquiryResponseDTO PaymentResponse = await _fatoorahService.GetPaymentResponse(PaymentId);
            RestaurantTransactionHistory transactionHistory = new();

            if (PaymentResponse.IsSuccess)
            {
                IEnumerable<SupplierOrder> Orders = await _supplierOrder.GetByIdAsync(OrderId);
                SupplierOrder order = Orders.FirstOrDefault();

                PaymentInquiryDataDTO Payment = PaymentResponse.Data;

                if ((Payment != null && Payment.InvoiceStatus == "Paid"))
                {
                    InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Succss").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

                    transactionHistory.NameOnCard = Payment.CustomerName;
                    transactionHistory.MaskCardNo = InvoiceTransaction.CardNumber;
                    transactionHistory.TransactionStatus = InvoiceTransaction.TransactionStatus;
                    transactionHistory.Amount = (decimal)Payment.InvoiceValue;
                    transactionHistory.SupplierOrderId = order.Id;
                    transactionHistory.RestaurantId = order.RestaurantId;

                    await _historyService.AddRestaurantTransactionHistoryAsync(transactionHistory);

                    order.IsPaid = true;
                    order.InvoiceRef = Payment.InvoiceReference;

                    await _supplierOrder.UpdateSupplierOrderAsync(order);

                    return Ok(1);
                }
                else if (Payment != null && Payment.InvoiceStatus == "Pending")
                {
                    order.IsPaid = false;
                    order.InvoiceRef = Payment.InvoiceReference;
                    order.PaymentCaptured = true;

                    await _supplierOrder.UpdateSupplierOrderAsync(order);

                    return Ok(2);
                }
                else
                {
                    order.IsPaid = false;
                    order.InvoiceRef = Payment.InvoiceReference;
                    order.PaymentCaptured = true;

                    await _supplierOrder.UpdateSupplierOrderAsync(order);

                    return Ok(3);
                }
            }
            return Conflict(3);
        }

    }
}