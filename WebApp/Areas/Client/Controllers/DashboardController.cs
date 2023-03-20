using AutoMapper;
using HelperClasses.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class DashboardController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGarageDashboardClient _client;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IGarageCustomerAppointmentClient _customerAppointmentClient;
        private readonly IGarageClient _garageClient;
        private readonly IClientModulesClient _clientModule;
        [BindProperty]
        public GarageDashboardViewModel Model { get; set; }
        public DashboardController( IMapper mapper, IUserSessionManager userSessionManager , IGarageDashboardClient client,
            ISupplierOrderClient orderClient, IGarageCustomerAppointmentClient customerAppointmentClient
            , IGarageClient garageClient, IClientModulesClient clientModule)
        {
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _client = client;
            _customerAppointmentClient = customerAppointmentClient;
            _garageClient = garageClient;
            _clientModule = clientModule;
        }
        public async Task<IActionResult> Index()
        {
            long GarageId = _userSessionManager.GetGarageStore().Id;
            //_mapper.Map<GarageDashboardViewModel>(await _clientModule.GetModuleByClientId(GarageId));
            var count = _mapper.Map<GarageDashboardViewModel>(await _client.GetGarageDashboardCount(GarageId));
            count.CustomerAppointment = _mapper.Map<List<GarageCustomerAppointmentViewModel>>(await _customerAppointmentClient.GetAllByGarageIdAsync(GarageId));
            return View(count);
        }
    }
}
