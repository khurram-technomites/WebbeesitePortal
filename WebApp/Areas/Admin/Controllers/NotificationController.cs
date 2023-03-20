using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Handlers;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.DTOs.NotificationFilter;
using WebApp.Interfaces;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager, Restaurant Manager, B2B Manager, GarageOwner , SparePartDealer,Vendor")]
    public class NotificationController : Controller
    {
        private readonly INotificationClient _notificationService;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IMapper _mapper;
        [BindProperty]
        public NotificationViewModel Model { get; set; }
        public NotificationController(INotificationClient notificationService, IMapper mapper, IUserSessionManager userSessionManager)
        {
            _notificationService = notificationService;
            _userSessionManager = userSessionManager;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> LoadNotifications(int pageNo)
        {
            NotificationFilterDTO Filter = new();

            Filter.Paging.PageNumber = pageNo;
            var Notifications = await _notificationService.GetNotification(Filter);
            return Json(new
            {
                success = true,
                message = "Data retrieved successfully!",
                data = Notifications
            });
        }

        [HttpPost]
        public async Task<ActionResult> LoadNotification(NotificationFilterDTO model)
        {
            PagingParameters pagging = new();
            pagging.PageSize = model.Paging.PageSize;
            pagging.PageNumber = model.Paging.PageNumber;
            model.Paging = pagging;
            var Notifications = await _notificationService.GetNotification(model);
            return Json(new
            {
                success = true,
                message = "Data retrieved successfully !",
                data = Notifications

            });

        }
        public async Task<ActionResult> MarkNotificationsAsSeen()
        {
            var MarkNotification = await _notificationService.MarkNotificationsAsSeen(_userSessionManager.GetUserStore().UserId);
            if (MarkNotification != null)
            {
                if (MarkNotification != null)
                {
                    return Json(new { success = true, message = "Notification seen successfully !" });
                }
                return Json(new { success = false, message = "Error !" });
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" });
            }
        }

        public async Task<ActionResult> MarkNotificationsAsRead(long notificationId)
        {
            var MarkNotification = await _notificationService.MarkNotificationsAsRead(notificationId);
            if (MarkNotification != null)
            {
                if (MarkNotification != null)
                {
                    return Json(new { success = true, message = "Notification seen successfully !" });
                }
                return Json(new { success = false, message = "Error !" });
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" });
            }
        }

        public async Task<ActionResult> ViewAllNotification()
        {
            NotificationFilterDTO model = new();
            model.Paging.PageSize = 10;
            model.Paging.PageNumber = 1;
            var Notifications = await _notificationService.GetNotification(model);

            return View(_mapper.Map<IEnumerable<NotificationReceiverViewModel>>(Notifications));
        }

        /*[HttpGet("{skipCount}/{takeCount}")]
        public async Task<IEnumerable<ActionResult>> ViewAllNotification(int skipCount, int takeCount)
        {
            NotificationFilterDTO model = new NotificationFilterDTO();
            model.Paging.Skip = skipCount;
            model.Paging.PageSize = takeCount;
            model.UserId = "181d4526-00d4-413b-a7d5-8ccf8f14af48";
            model.Paging.PageNumber = 1;
            var Notifications = await _notificationService.GetNotification(model);
            return _mapper.Map<IEnumerable<NotificationReceiverViewModel>>(Notifications);
        }*/
        public async Task<IActionResult> Details(long Id)
        {
            NotificationViewModel a = _mapper.Map<NotificationViewModel>(await _notificationService.GetNotificationByID(Id));
            return View(a);
        }
        public ActionResult NotificationsReadAll()
        {
            var MarkNotification = _notificationService.MarkNotificationsAsSeen(_userSessionManager.GetUserStore().UserId);
            if (MarkNotification != null)
            {
                if (MarkNotification != null)
                {
                    return Json(new { success = true, message = "Notification seen successfully !" });
                }
                return Json(new { success = false, message = "Error !" });
            }
            else
            {
                return Json(new { success = false, message = "Authorization failed!" });
            }
        }

    }
}
