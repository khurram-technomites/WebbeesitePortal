using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    [Authorize(Roles = "RestaurantOwner")]
    public class RestaurantSubscriberController : Controller
    {
        private readonly ISubscriberClient _subService;
        private readonly IRestaurantSubscriberClient _subscriberClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;

        public RestaurantSubscriberController(ISubscriberClient subService, IMapper mapper, IRestaurantSubscriberClient subscriberClient, IUserSessionManager userSession)
        {
            _subService = subService;
            _mapper = mapper;
            _subscriberClient = subscriberClient;
            _userSession = userSession;
        }
        public async Task<IActionResult> Index()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            return View(await _subscriberClient.GetByRestaurant(RestaurantId));
        }
        public async Task<ActionResult> SendEmailToSubscribers()
        {
            long RestaurantId = _userSession.GetUserStore().Id;
            ViewBag.Email = await _subscriberClient.GetByRestaurant(RestaurantId);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendEmailToSubscribers(RestaurantSubcriberViewModel subscribermail)
        {
            string message = string.Empty;
            foreach (var item in subscribermail.Email)
            {
                await _subscriberClient.SendMessage(subscribermail.Email, subscribermail.Message);

              /*  if (_subService.SendMessage(subscribermail.Email , subscribermail.Subject , subscribermail.Message))
                {
                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Subscriber/Index",
                        message = "Email Sent",
                        data = new
                        {

                            Date = Email.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            Email = Email.Email,



                        }
                    });

                }*/
            }

            return RedirectToAction("Index", "Subscriber");
        }
        public async Task<ActionResult> DeleteSubscriber(long Id)
        {
            try
            {
                await _subscriberClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }
    }
}
