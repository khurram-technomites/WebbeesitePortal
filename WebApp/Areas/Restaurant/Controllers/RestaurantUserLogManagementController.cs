using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantUserLogManagementController : Controller
    {
        private readonly IRestaurantUserLogManagementClient _client;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IRestaurantBranchClient _restaurantBranchClient;
        public RestaurantUserLogManagementController(IRestaurantUserLogManagementClient client, IMapper mapper, IUserSessionManager userSessionManager, IRestaurantBranchClient restaurantBranchClient)
        {
            _client = client;
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _restaurantBranchClient = restaurantBranchClient;
        }

        public async Task<IActionResult> Index()
        {
            //PagingParameters paging = new PagingParameters();
            //paging.PageNumber = 1;
            //paging.PageSize = 10;

            long restaurantId = _userSessionManager.GetUserStore().Id;
            var Info = _mapper.Map<IEnumerable<RestaurantUserLogManagementViewModel>>(await _client.GetUserLogManagementByRestaurantIdAsync(restaurantId));
            return View(Info);
        }
    }
}
