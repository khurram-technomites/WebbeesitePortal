using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class WebViewController : Controller
    {
        private readonly ICustomerRestaurantClient _client;
        private readonly IGarageCustomerClient _garageClient;
        private readonly IClientModulePurchasesClient _purchasesClient;
        private readonly IMapper _mapper;
        public WebViewController(ICustomerRestaurantClient client, IMapper mapper , IGarageCustomerClient garageClient, IClientModulePurchasesClient purchasesClient)
        {
            _client = client;
            _mapper = mapper;
            _garageClient = garageClient;
            _purchasesClient = purchasesClient;
        }

        public IActionResult Index(long OrderId, [FromQuery] string PaymentId)
        {
            ViewBag.OrderId = OrderId;
            ViewBag.PaymentId = PaymentId;
            return View();
        }
        public IActionResult GarageInvoiceIndex(long InvoiceId, [FromQuery] string PaymentId)
        {
            ViewBag.InvoiceId = InvoiceId;
            ViewBag.PaymentId = PaymentId;
            return View();
        }
        public IActionResult ClientInvoiceIndex(long InvoiceId, [FromQuery] string PaymentId)
        {
            ViewBag.OrderId = InvoiceId;
            ViewBag.PaymentId = PaymentId;
            return View();
        }
        public async Task<IActionResult> GetGarageStatusDetail(long InvoiceId, string PaymentId)
        {
            try
            {
                object result = await _garageClient.Paid(InvoiceId, PaymentId);
                return Json(new
                {
                    success = true,
                    data = result
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                });
            }

        }
        public async Task<IActionResult> GetClientStatusDetail(long InvoiceId, string PaymentId)
        {
            try
            {
                object result = await _purchasesClient.Paid(InvoiceId, PaymentId);
                return Json(new
                {
                    success = true,
                    data = result
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                });
            }

        }
        public async Task<IActionResult> GetStatusDetail(long OrderId, string PaymentId)
        {           
            try
            {
                object result = await _client.Paid(OrderId, PaymentId);
                return Json(new
                {
                    success = true,
                    data = result
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                });
            }

        }

    }
}
