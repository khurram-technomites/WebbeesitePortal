using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class ClientTransactionsController : Controller
    {
        private readonly IUserSessionManager _sessionManager;
        private readonly IMapper _mapper;
        private readonly IClientModulePurchaseTransactionsClient _clientModulePurchaseTransactionsClient;
        public ClientTransactionsController(IUserSessionManager userSessionManager, IMapper mapper, IClientModulePurchaseTransactionsClient clientModulePurchaseTransactionsClient)
        {
            _sessionManager = userSessionManager;
            _mapper = mapper;
            _clientModulePurchaseTransactionsClient = clientModulePurchaseTransactionsClient;
        }
        public async Task<IActionResult> Index()
        {
            long VendorId = _sessionManager.GetVendorStore().Id;
            var info = _mapper.Map<IEnumerable<ClientModulePurchaseTransactionsViewModel>>(await _clientModulePurchaseTransactionsClient.GetAllTransactionsAsync(VendorId));
            return View(info);
        }
        public async Task<IActionResult> List()
        {
            return View();
        }
    }
}
