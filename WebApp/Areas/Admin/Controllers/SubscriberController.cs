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
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberClient _subService;
        private readonly IMapper _mapper;

        public SubscriberController(ISubscriberClient subService, IMapper mapper)
        {
            _subService = subService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.ToDate = Helpers.TimeZone.GetLocalDateTime().ToString("MM/dd/yyyy");
            ViewBag.FromDate = Helpers.TimeZone.GetLocalDateTime().AddDays(-30).ToString("MM/dd/yyyy");
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            var info = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subService.GetSubscribers(pagging));
            return View(info);
        }
        public async Task<ActionResult> List()
        {
            return PartialView();
        }
        public async Task<ActionResult> SendEmailToSubscribers()
        {
            PagingParameters pagging = new PagingParameters();
            pagging.PageSize = 10;
            pagging.PageNumber = 1;
            ViewBag.Email = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subService.GetSubscribers(pagging));
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SendEmailToSubscribers(SubscriberViewModel subscribermail)
        {
            string message = string.Empty;
            foreach (var item in subscribermail.Email)
            {
                SubscriberDTO Email = await _subService.GetEmailByID(item);

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
                await _subService.Delete(Id);

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

        [HttpPost]
        public async Task<ActionResult> List(DateTime fromDate, DateTime ToDate)
        {
            DateTime EndDate = ToDate.AddMinutes(1439);
            var Subscribers = _subService.GetsubscribersDateWise(fromDate, EndDate);
            return PartialView();
        }

    }

}
