using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnnoucementController : Controller
    {
        private readonly ICustomerClient _customerClient;
        private readonly IRestaurantRatingClient _restaurantRatingClient;
        private readonly IGarageClient _garageClient;
        private readonly ISparePartsDealerClient _sparePartsDealerClient;
        private readonly IMapper _mapper;
        public AnnoucementController(IMapper mapper, ICustomerClient customerClient, IRestaurantRatingClient restaurantRatingClient, IGarageClient garageClient, ISparePartsDealerClient sparePartsDealerClient)
        {
            _customerClient = customerClient;
            _restaurantRatingClient = restaurantRatingClient;
            _garageClient = garageClient;
            _sparePartsDealerClient = sparePartsDealerClient;
            _mapper = mapper;
        }
        public async Task<ActionResult> SendNotification()
        {
            ViewBag.CustomerID = _mapper.Map<IEnumerable<CustomerViewModel>>(await _customerClient.GetAllCustomersAsync());
            ViewBag.RestaurantID = _mapper.Map<IEnumerable<RestaurantRatingViewModel>>(await _restaurantRatingClient.GetRestaurantRatings());
            ViewBag.GarageID = _mapper.Map<IEnumerable<GarageViewModel>>(await _garageClient.GetGarages());
            ViewBag.SparePartDealerID = _mapper.Map<IEnumerable<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationViewModel notificationViewModel)
        {
            try
            {
                string message = string.Empty;
                for (int i = 0; i < notificationViewModel.Customers.Count; i++)
                {
                    /* var tokn = _customerClient.*/
                }
                return Json(new { success = true, message = "Notification sent sucessfully... " });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error !" });
            }
        }
    }
}
