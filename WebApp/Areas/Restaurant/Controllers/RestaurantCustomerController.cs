using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantCustomerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClient _client;
        private readonly IUserSessionManager _userSession;
        [BindProperty]
        public CustomerViewModel Model { get; set; }
        public RestaurantCustomerController(IMapper mapper, ICustomerClient client, IUserSessionManager userSession)
        {
            _mapper = mapper;
            _client = client;
            _userSession = userSession;
        }
        public async Task<IActionResult> Index()
        {
            long restaurantId = _userSession.GetUserStore().Id;
            var user = _mapper.Map<IEnumerable<RestaurantCustomerViewModel>>(await _client.GetAllCustomersByRestaurantAsync(restaurantId));
            return View(user);
        }

        public async Task<IActionResult> Details(long Id)
        {
            CustomerViewModel CustomerDetail = _mapper.Map<CustomerViewModel>(await _client.GetCustomerByIdAsync(Id));
            return View(CustomerDetail);
        }
    }
}
