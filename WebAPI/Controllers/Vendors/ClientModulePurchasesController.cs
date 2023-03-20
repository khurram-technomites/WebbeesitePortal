using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Fatoorah;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Vendor,GarageOwner")]
    public class ClientModulePurchasesController : Controller
    {
        private readonly IClientModulePurchasesService _clientModulePurchasesService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        private readonly IFatoorahService _fatoorahService;
        private readonly IConfiguration _config;
        private readonly IModulePurchaseDetailsService _modulePurchaseDetailsService;
        private readonly IClientModulesService _clientModulesService;
        private readonly IClientModulePurchaseTransactionsService _clientModulePurchaseTransactionsService;
        private readonly IGarageService _garageService;
        public ClientModulePurchasesController(IClientModulePurchasesService clientModulePurchasesService, IMapper mapper, IFTPUpload fTPUpload, IFatoorahService fatoorahService, IConfiguration config, IModulePurchaseDetailsService modulePurchaseDetailsService,IClientModulesService clientModulesService, IClientModulePurchaseTransactionsService clientModulePurchaseTransactionsService, IGarageService garageService)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _clientModulePurchasesService = clientModulePurchasesService;
            _fatoorahService = fatoorahService;
            _config = config;
            _modulePurchaseDetailsService = modulePurchaseDetailsService;
            _clientModulesService = clientModulesService;
            _clientModulePurchaseTransactionsService = clientModulePurchaseTransactionsService;
            _garageService = garageService;
        }


        [HttpGet("GarageId/{GarageId}")]
        public async Task<IActionResult> GetDetailsByPurchaseId(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<ClientModulePurchasesDTO>>(await _clientModulePurchasesService.GetPurchaseByGarageId(GarageId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetailsByIdAsync(long Id)
        {
            IEnumerable<ClientModulePurchasesDTO> PurchaseDetails = _mapper.Map<IEnumerable<ClientModulePurchasesDTO>>(await _clientModulePurchasesService.GetPurchaseByIdAsync(Id));
            return Ok(PurchaseDetails.FirstOrDefault());
        }
        [HttpPost("Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> AddInvoice(ClientModulePurchasesDTO clientModulePurchasesDTO)
        {
            try
            {
                string garagePath = string.Empty;
                if (clientModulePurchasesDTO.Garage != null)
                {
                    garagePath = string.Format("{0}/WebView/ClientInvoiceIndex?InvoiceId={1}", _config.GetValue<string>("WebAppURL"), clientModulePurchasesDTO.Id);
                }
                var result = new SuccessResponse<string>("", await _fatoorahService.InitiatePayment(clientModulePurchasesDTO, garagePath));
                if (result.Status == "Success")
                    clientModulePurchasesDTO.PaymentStatus = Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.UnPaid);
                    clientModulePurchasesDTO.PaymentUrl = result.Result;
                await _clientModulePurchasesService.UpdateAsync(_mapper.Map<ClientModulePurchases>(clientModulePurchasesDTO));
                {
                    return Ok(new
                    {
                        Status = "Success",
                        Data = clientModulePurchasesDTO.PaymentUrl
                    }); ;
                }
                return Ok(new
                {
                    Status = "Failed"
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    Status = "Failed"
                });
            }
            
        }
        [HttpGet("Paid/{InvoiceId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Paid(long InvoiceId, [FromQuery(Name = "PaymentId")] string PaymentId)
        {

            List<NotificationReceiver> notificationReceivers = new();
            PaymentInquiryResponseDTO PaymentResponse = await _fatoorahService.GetPaymentResponse(PaymentId);
            ClientModulePurchases invoice = new();
            ClientModulePurchaseTransactions transactions = new ClientModulePurchaseTransactions();

            if (PaymentResponse.IsSuccess)
            {
                IEnumerable<ClientModulePurchases> invoices = await _clientModulePurchasesService.GetPurchaseByIdAsync(InvoiceId);
                 invoice = invoices.FirstOrDefault();
                if(invoice.PaymentStatus == "Paid")
                {
                    return Ok(new SuccessResponse<object>("Payment successful", new
                    {
                        paymentStatus = "Paid",
                        PaymentUrl = string.Format("{0}/Vendor/Dashboard/Index", _config.GetValue<string>("WebAppURL")),
                        totalAmount = invoice.Total,
                        orderId = invoice.Id
                    }));

                }
                PaymentInquiryDataDTO Payment = PaymentResponse.Data;
                if ((Payment != null && Payment.InvoiceStatus == "Paid"))
                {
                    InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Succss").OrderByDescending(i => i.TransactionDate).FirstOrDefault();
                    var garage =(await _garageService.GetGarageByIdAsync(invoice.GarageID));
                    Garage _garage = garage.FirstOrDefault();
                    _garage.Status = Enum.GetName(typeof(Status), Status.Processing);
                  


                    invoice.PaymentStatus = Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid);
                    invoice.SubTotal = (decimal)Payment.InvoiceValue;
                    invoice.PaymentInvoiceID = PaymentId;
                    invoice.PaymentRef = PaymentId;
                    await _clientModulePurchasesService.UpdateAsync(invoice);

                    var modules = await _modulePurchaseDetailsService.GetDetailsByPurchaseId(InvoiceId);
                    var oldpackage = _clientModulesService.GetClientModuleByClientIdAsync(InvoiceId).Result.LastOrDefault();
                    List<ClientModules> clientModules = new List<ClientModules>();
                    foreach (var module in modules)
                    {
                        //IF Package Exist 
                        ClientModules oldclientmodule =  _clientModulesService.GetClientModuleByModuleIdAsync(module.ModuleID, invoice.GarageID).Result.LastOrDefault();
                        if (oldclientmodule != null)
                        {
                            oldclientmodule.Quantity = module.Module.ManageQunatity == false ? 0 : oldclientmodule.Quantity + module.Quantity;
                            oldclientmodule.TotalPrice = oldclientmodule.TotalPrice + module.TotalPrice;
                            clientModules.Add(oldclientmodule);
                        }
                        else
                        {
                            var _clientModules = new ClientModules();
                            _clientModules.ClientId = invoice.GarageID;
                            _clientModules.ModuleId = module.ModuleID;
                            _clientModules.Quantity = module.Module.ManageQunatity == false? 0: module.Quantity;
                            _clientModules.TotalPrice = module.TotalPrice;
                            _clientModules.PurchaseDate = DateTime.UtcNow.ToDubaiDateTime();
                            if (oldpackage != null)
                            {
                                var noofdayleft = (oldpackage.ExpiryDate - DateTime.UtcNow).Days;
                                _clientModules.ExpiryDate = DateTime.UtcNow.AddDays(noofdayleft);
                            }
                            else
                            {
                                _clientModules.ExpiryDate = DateTime.UtcNow.AddDays(365); 
                            }
                            _clientModules.Status = Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Paid);
                            clientModules.Add(_clientModules);
                        }
                        

                        if (module.Module.ServiceName == ModulesObject.Services)
                        {
                            _garage.IsServicesAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Blogs)
                        {
                            _garage.IsBlogsAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Appoinmnets)
                        {
                            _garage.IsAppoinmnetsAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Teams)
                        {
                            _garage.IsTeamsAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Feedback)
                        {
                            _garage.IsFeedbackAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Careers)
                        {
                            _garage.IsCareersAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Expertis)
                        {
                            _garage.IsExpertisAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Partner)
                        {
                            _garage.IsPartnerAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Project)
                        {
                            _garage.IsProjectAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Testimonial)
                        {
                            _garage.IsTestimonialAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.Award)
                        {
                            _garage.IsAwardAllowed = true;
                        }
                        else if (module.Module.ServiceName == ModulesObject.CustomerAppoinment)
                        {
                            _garage.IsCustomerAppoinmentAllowed = true;
                        }

                    }
                    await _garageService.UpdateGarageAsync(_mapper.Map<Garage>(_garage));
                    clientModules.ForEach(x => x.Garage = null);
                    await _clientModulesService.UpdateClientModuleRangeAsync(clientModules);

                    transactions.ClientModulePurchaseID = InvoiceId;
                    transactions.Amount = (decimal)Payment.InvoiceValue;
                    transactions.NameOnCard = Payment.CustomerName;
                    transactions.MaskCardNo = InvoiceTransaction.CardNumber;
                    transactions.PaymentStatus = InvoiceTransaction.TransactionStatus;

                    await _clientModulePurchaseTransactionsService.AddClientModulePurchaseTransactionsAsync(transactions);

                    

                    return Ok(new SuccessResponse<object>("Payment successful", new
                    {
                        paymentStatus = "Paid",
                        PaymentUrl = string.Format("{0}/Vendor/Dashboard/Index", _config.GetValue<string>("WebAppURL")),
                        totalAmount = invoice.Total,
                        orderId = invoice.Id
                    }));
                }
                else if (Payment != null && Payment.InvoiceStatus == "Pending")
                {
                    InvoiceTransactionDTO InvoiceTransaction = Payment.InvoiceTransactions.Where(i => i.TransactionStatus == "Failed").OrderByDescending(i => i.TransactionDate).FirstOrDefault();

                    if (InvoiceTransaction != null)
                    {
                        invoice.PaymentStatus = Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Pending);
                        invoice.SubTotal = (decimal)Payment.InvoiceValue;
                        invoice.PaymentInvoiceID = PaymentId;
                        invoice.PaymentRef = PaymentId;

                        await _clientModulePurchasesService.UpdateAsync(invoice);


                        transactions.ClientModulePurchaseID = InvoiceId;
                        transactions.Amount = (decimal)Payment.InvoiceValue;
                        transactions.NameOnCard = Payment.CustomerName;
                        transactions.MaskCardNo = InvoiceTransaction.CardNumber;
                        transactions.PaymentStatus = InvoiceTransaction.TransactionStatus;

                        await _clientModulePurchaseTransactionsService.AddClientModulePurchaseTransactionsAsync(transactions);

                        return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
                        {
                            paymentStatus = "Failed",
                            PaymentUrl = invoice.PaymentUrl,
                            totalAmount = invoice.Total,
                            orderId = invoice.Id
                        }));
                    }
                }
                else
                {
                    invoice.PaymentStatus = Enum.GetName(typeof(ClientPaymentStatus), ClientPaymentStatus.Pending);
                    invoice.SubTotal = (decimal)Payment.InvoiceValue;
                    invoice.PaymentInvoiceID = PaymentId;
                    invoice.PaymentRef = PaymentId;

                    await _clientModulePurchasesService.UpdateAsync(invoice);

                    return Ok(new SuccessResponse<object>("Oops! Payment failed.Please try later", new
                    {
                        paymentStatus = "Failed",
                        PaymentUrl = invoice.PaymentUrl,
                        totalAmount = invoice.Total,
                        orderId = invoice.Id
                    }));


                }
            }
            return Conflict(new ErrorDetails(409, "Payment failed", ""));
        }
       [HttpPost]
        public async Task<IActionResult> Post(ClientModulePurchasesDTO Model)
        {

            return Ok(_mapper.Map<ClientModulePurchasesDTO>(await _clientModulePurchasesService.AddAsync(_mapper.Map<ClientModulePurchases>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClientModulePurchasesDTO Model)
        {

            //IEnumerable<ModulePurchaseDetailsDTO> List = await _modulePurchaseDetailsService.GetDetailsByIdAsync(Model.Id);
            return Ok(_mapper.Map<ClientModulePurchasesDTO>(await _clientModulePurchasesService.UpdateAsync(_mapper.Map<ClientModulePurchases>(Model))));
        }


    
    }
}
