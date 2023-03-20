using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using Microsoft.Extensions.Logging;
using HelperClasses.DTOs.Fatoorah;
using WebAPI.Helpers;
using WebAPI.ErrorHandling;
using HelperClasses.DTOs.Garage.Filter;
using System.Globalization;

namespace WebAPI.Controllers.Garages
{
    [Route("api/GarageCustomer/Invoice")]
    [ApiController]
    [Authorize]
    public class GarageCustomerController : ControllerBase
    {
        private readonly IGarageCustomerService _customerService;
        private readonly IGarageService _garagService;
        private readonly IMapper _mapper;
        private readonly IFatoorahService _fatoorahService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMessageService _messageService;
        private readonly ILogger<GarageCustomerController> _logger;
        private readonly INumberRangeService _numberRangeService;
        private readonly IFCMUserSessionService _fCMUserSession;
        private readonly IIntegrationSettingService _integrationSettingService;
        private readonly INotificationService _notificationService;
        public GarageCustomerController(IGarageCustomerService customerService,
            IMapper mapper, UserManager<AppUser> userManager, IMessageService messageService,
            IConfiguration config, IGarageService garagService, IFatoorahService fatoorahService,
            ILogger<GarageCustomerController> logger , INumberRangeService numberRangeService,
            IFCMUserSessionService fCMUserSession, IIntegrationSettingService integrationSettingService,
            INotificationService notificationService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
            _garagService = garagService;
            _fatoorahService = fatoorahService;
            _messageService= messageService;
            _logger = logger;
            _numberRangeService = numberRangeService;
            _fCMUserSession = fCMUserSession;
            _integrationSettingService = integrationSettingService;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Invoices = _mapper.Map<IEnumerable<GarageCustomerInvoiceDTO>>(await _customerService.GetAllCustomerInvoicesAsync());
            return Ok(new { Status = "Success", Data = Invoices });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<GarageCustomerInvoiceDTO> List = _mapper.Map<IEnumerable<GarageCustomerInvoiceDTO>>(await _customerService.GetCustomerInvoiceByIdAsync(Id));
            return Ok(new { Status = "Success", Data = List.FirstOrDefault() });
        }
        [HttpPost("MyWallet/Filter/{GarageId}")]
        public async Task<IActionResult> MyWallet(long GarageId, GarageWalletFilter Fiter)
        {
            IEnumerable<GarageCustomerInvoiceDTO> result = _mapper.Map<IEnumerable<GarageCustomerInvoiceDTO>>(await _customerService.GetAllWalletForFilterAsync(Fiter, GarageId));
            decimal walletSum = _mapper.Map<decimal>(await _customerService.getWallet(GarageId));
            return Ok(new { Status = "Success", Data = result, Wallet = walletSum.ToString("F", CultureInfo.InvariantCulture) });
        }
        [HttpGet("MyWallet/{GarageId}")]
        public async Task<IActionResult> MyWallet(long GarageId)
        {
            IEnumerable<GarageCustomerInvoiceDTO> List = _mapper.Map<IEnumerable<GarageCustomerInvoiceDTO>>(await _customerService.GetCustomerInvoiceByGarageIdAsync(GarageId));
            decimal walletSum = _mapper.Map<decimal>(await _customerService.getWallet(GarageId));
            return Ok(new { Status = "Success", Data = List , Wallet = walletSum.ToString("F", CultureInfo.InvariantCulture) });
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            var deletion = _mapper.Map<GarageCustomerInvoiceDTO>(await _customerService.ArchiveCustomerInvoiceAsync(Id));
            return Ok(new SuccessResponse<object>("Record Deleted Successfully", deletion));
        }
        [HttpPost("Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> AddInvoice(GarageCustomerInvoiceDTO invoiceDTO, [FromQuery] Client Client = 0)
        {
            List<NotificationReceiver> notificationReceivers = new();
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            decimal DiscountAmount = 0;
            GarageCustomerInvoice garageInvoice = new();
            garageInvoice = _mapper.Map<GarageCustomerInvoice>(invoiceDTO);
            if (!garageInvoice.CustomerPhoneNumber.StartsWith("+"))
            {
                garageInvoice.CustomerPhoneNumber = "+" + garageInvoice.CustomerPhoneNumber;
            }
            garageInvoice.Total = garageInvoice.Total + garageInvoice.ServiceFee - garageInvoice.Discount;
            garageInvoice.Status = Enum.GetName(typeof(GarageCustomerStatus), GarageCustomerStatus.UnPaid);
            garageInvoice.InvoiceNo = await _numberRangeService.GetNextRange("GINV-");
            GarageCustomerInvoiceDTO invoice = _mapper.Map<GarageCustomerInvoiceDTO>(await _customerService.AddCustomerInvoiceAsync(_mapper.Map<GarageCustomerInvoice>(garageInvoice)));
            if (garageInvoice.Status == "UnPaid")
            {
                string garagePath = string.Empty;
                IEnumerable<WebAPI.Models.Garage> Garages = await _garagService.GetGarageByIdAsync(invoiceDTO.GarageId);

                var garage = Garages.FirstOrDefault();
                if (garage != null)
                {
                    garagePath = string.Format("{0}/WebView/GarageInvoiceIndex?InvoiceId={1}", _config.GetValue<string>("WebAppURL"), invoice.Id);
                }
                var result = new SuccessResponse<string>("", await _fatoorahService.InitiateGaragePayment(invoice, garagePath));
                if (result.Status == "Success")
                {
                    try
                    {
                        string Text = "Click this link to pay " + result.Result ;
                        if (await _messageService.SendMessage(invoice.CustomerPhoneNumber, Text))
                        {
                            _logger.LogInformation("Message has been sent successfully " + invoice.CustomerPhoneNumber);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Message Sending Failed for " + invoice.CustomerPhoneNumber + " with message: " +
                                          ex.Message);
                    }
                }
                return Ok(new
                {
                    Status = "Success",
                    Data = invoice
                });
            }
            //Model.Status = Enum.GetName(typeof(GarageCustomerInvoice), GarageCustomerStatus.Paid);

            return Ok(new
            {
                Status = "Success",
                Data = invoice
            });
        }
        [HttpGet("Paid/{InvoiceId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Paid(long InvoiceId, [FromQuery(Name = "PaymentId")] string PaymentId)
        {
            string origin = Request.Headers["origin"];

            List<NotificationReceiver> notificationReceivers = new();
            PaymentInquiryResponseDTO PaymentResponse = await _fatoorahService.GetPaymentResponse(PaymentId);
            GarageCustomerInvoice transactionHistory = new();

            if (PaymentResponse.IsSuccess)
            {
                IEnumerable<GarageCustomerInvoice> invoices = await _customerService.GetCustomerInvoiceByIdAsync(InvoiceId);
                GarageCustomerInvoice invoice = invoices.FirstOrDefault();
                PaymentInquiryDataDTO Payment = PaymentResponse.Data;
                if ((Payment != null && Payment.InvoiceStatus == "Paid"))
                {
                    InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Succss").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

                    transactionHistory.NameOnCard = Payment.CustomerName;
                    transactionHistory.MaskCardNo = InvoiceTransaction.CardNumber;
                    transactionHistory.Status = InvoiceTransaction.TransactionStatus;
                    transactionHistory.Total = (decimal)Payment.InvoiceValue;
                    //transactionHistory.OrderId = invoice.Id;
                    transactionHistory.PaymentId = PaymentId;
                    transactionHistory.Origin = string.IsNullOrEmpty(origin) ? null : origin;

                    await _customerService.AddCustomerInvoiceAsync(transactionHistory);

                    invoice.IsPaid = true;
                    invoice.PaymentCaptured = true;
                    invoice.InvoiceRef = PaymentId;

                    await _customerService.UpdateCustomerInvoiceAsync(invoice);

                    if (!invoice.IsPaid)
                    {
                        List<FCMUserSession> FCMList = new();

                        if (FCMList.Any())
                        {
                            string[] tokens = FCMList.Select(x => x.FirebaseToken).ToArray();
                            IEnumerable<IntegrationSetting> settings = await _integrationSettingService.GetAllAsync();
                            var response = PushNotifications.SendPushNotification(tokens, string.Format("Hey! New Invoice ({1}) created",  invoice.InvoiceNo), "", new
                            {
                                invoice.InvoiceNo,
                                OrderId = invoice.Id
                            }, settings.FirstOrDefault().PartnerFCMKey, false);
                        }

                        if (invoice != null)
                        {
                            notificationReceivers.Add(new NotificationReceiver
                            {
                                ReceiverId = invoices.FirstOrDefault().Garage.UserId,
                                IsSeen = false,
                                IsDelivered = false,
                                IsRead = false,
                                ReceiverType = Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff),
                            });

                            Notification notification = new()
                            {
                                OriginatorId = "",
                                OriginatorName = invoice.CustomerName,
                                Description = "Invoice Has Been Paid",
                                RecordId = invoice.Id,
                                //OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                                //Url = "/Restaurant/RestaurantOrder/Index",
                                NotificationReceivers = notificationReceivers
                            };

                            await _notificationService.AddNotification(notification);
                        }
                    }

                    return Ok(new SuccessResponse<object>("Payment successful", new
                    {
                        paymentStatus = "Paid",
                        orderNo = invoice.InvoiceNo,
                        totalAmount = invoice.Total,
                        orderId = invoice.Id
                    }));
                }
                else if (Payment != null && Payment.InvoiceStatus == "Pending")
                {
                    InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Failed").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

                    if (InvoiceTransaction != null)
                    {
                        transactionHistory.NameOnCard = Payment.CustomerName;
                        transactionHistory.MaskCardNo = InvoiceTransaction.CardNumber;
                        transactionHistory.Status = InvoiceTransaction.TransactionStatus;
                        transactionHistory.Total = (decimal)Payment.InvoiceValue;
                        transactionHistory.OrderId = invoice.Id;
                        transactionHistory.PaymentId = PaymentId;
                        transactionHistory.GarageId = invoice.GarageId;
                        transactionHistory.Origin = string.IsNullOrEmpty(origin) ? null : origin;

                        if (invoice.CustomerId.HasValue)
                        {
                            transactionHistory.CustomerId = invoice.CustomerId.Value;
                        }

                        await _customerService.AddCustomerInvoiceAsync(transactionHistory);

                        invoice.IsPaid = false;
                        invoice.InvoiceRef = PaymentId;
                        invoice.PaymentCaptured = false;
                        //cancel order
                        invoice.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
                        //order.Restaurant = null;
                        //order.RestaurantBranch = null;

                        await _customerService.UpdateCustomerInvoiceAsync(invoice);

                        return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
                        {
                            paymentStatus = "Failed",
                            orderNo = invoice.InvoiceNo,
                            totalAmount = invoice.Total,
                            orderId = invoice.Id
                        }));
                    }
                    else
                    {
                        invoice.IsPaid = false;
                        invoice.InvoiceRef = PaymentId;
                        invoice.PaymentCaptured = true;
                        invoice.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);

                        await _customerService.UpdateCustomerInvoiceAsync(invoice);

                        return Ok(new SuccessResponse<object>("Payment is in process", new
                        {
                            paymentStatus = "Pending",
                            orderNo = invoice.InvoiceNo,
                            totalAmount = invoice.Total,
                            orderId = invoice.Id
                        }));
                    }
                }
                else
                {
                    invoice.IsPaid = false;
                    invoice.InvoiceRef = PaymentId;
                    invoice.PaymentCaptured = false;
                    invoice.Status = Enum.GetName(typeof(OrderStatus), OrderStatus.Canceled);
                    await _customerService.UpdateCustomerInvoiceAsync(invoice);

                    return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
                    {
                        paymentStatus = "Failed",
                        orderNo = invoice.InvoiceNo,
                        totalAmount = invoice.Total,
                        orderId = invoice.Id
                    }));


                }
            }
            return Conflict(new ErrorDetails(409, "Payment failed", ""));
        }
    }
}
