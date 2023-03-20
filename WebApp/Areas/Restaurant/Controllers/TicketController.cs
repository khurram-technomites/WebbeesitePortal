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
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Restaurant.Controllers
{
	[Area("Restaurant")]
	[Authorize(Roles = "RestaurantOwner , Supplier")]

	public class TicketController : Controller
    {
		private readonly IMapper _mapper;
		private readonly ITicketClient _client;
		private readonly IUserSessionManager _userSession;
        private readonly ITicketMessageClient _ticketmessageService;
        private readonly INotificationClient _notificationClient;
        private readonly IFileUpload _fileUpload;

		public TicketController(IMapper mapper, ITicketClient client, IUserSessionManager userSession , INotificationClient notificationClient , IFileUpload fileUpload , ITicketMessageClient ticketMessageClient)
		{
			_mapper = mapper;
			_client = client;
			_userSession = userSession;
			_notificationClient = notificationClient;
            _ticketmessageService = ticketMessageClient;
            _fileUpload = fileUpload;
		}
		public async Task<IActionResult> Index()
        {
			long restaurantId = _userSession.GetUserStore().Id;
			var info = _mapper.Map<IEnumerable<TicketViewModel>>(await _client.GetTicketsByRestaurant(restaurantId));
            return View(info);
		}
		public async Task<IActionResult> Details(long Id)
		{
            //var conversation = _ticketmessageService.GetTicketConversation(id);
            TicketViewModel Details = new TicketViewModel();
            IEnumerable<TicketMessagesDTO> conversation = await _ticketmessageService.GetTicketMessageByTicketId(Id);
            TicketViewModel a = _mapper.Map<TicketViewModel>(await _client.GetTicket(Id));
            a.ticketConversation = _mapper.Map<List<TicketMessageViewModel>>(conversation);
			return View(a);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(TicketViewModel model)
		{
			model.RestaurantId = _userSession.GetUserStore().Id;
			model.UserId = _userSession.GetUser().UserId;
            model.CreationDate = DateTime.UtcNow;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    TicketDTO Result = await _client.CreateTicket(model);
                    //NotificationViewModel not = new NotificationViewModel();
                    //not.Title = "Ticket";
                    //not.Description = "New ticket generated";
                    //not.OriginatorId = _userSession.GetUserStore().UserId;
                    //not.OriginatorName = _userSession.GetUserStore().NameArAsPerTradeLicense;
                    //not.Url = "/Admin/Ticket/Index";
                    //not.Module = "Ticket";
                    //not.OriginatorType = "RestaurantOwner";
                    //not.RecordId = model.Id;
                    //var createNotification = await _notificationClient.Create(not);
                    //if (createNotification != null)
                    //{
                    //    //if (_notificationClient.GetNotification(not.Id, "admin", not.OriginatorId))
                    //    //{
                    //    //}
                    //}
                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/Ticket/Index",
                        message = "Ticket Created Successfully",
                        data = new
                        {
                            ID = model.Id,
                            Description = model.Description,
                            Priority = model.Priority,
                            Status = model.Status,
                            Date = model.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                            TicketNo = model.TicketNo == null ? "-" : model.TicketNo,


                        }
                    });
                }
                catch (ApiException ex)
                {
                    ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                    return Json(new
                    {
                        url = "/Restaurant/Ticket/Index",
						success = false,
                        message = err.Message
                    });
                }
            }

            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Message(TicketMessageViewModel model, string File)
        {
            //string message = string.Empty;
            model.SenderId = _userSession.GetUserStore().Id.ToString();
            model.SenderType = _userSession.GetUserStore().NameAsPerTradeLicense;
            try
            {
                if (File != "undefined" && File != null)
                {
                    model.TicketDocument.URL = File;
                }
                else if (File == null && model.Message == null)
                {
                    model.TicketDocument = null;
                    return Json(new
                    {
                        success = false,
                        message = "Message Sending Failed!",
                    });
                }
                else if (File == null && model.Message != null)
                {
                    model.TicketDocument = null;
                }
                else
                {
                    model.TicketDocument = null;
                }
                if (model.Message != null)
                {
                    model.CreationDate = DateTime.UtcNow;
                    TicketMessagesDTO Result = await _ticketmessageService.CreateTicketMessage(model);

                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/Ticket/Index",
                        message = "Message Sent Successfully",
                        data = Result,
                        creationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt")
                    });
                }
                else if(model.Message == null && File != "undefined" && File != null)
                {
                    model.Message = string.Empty;
                    model.CreationDate = DateTime.UtcNow;
                    TicketMessagesDTO Result = await _ticketmessageService.CreateTicketMessage(model);

                    return Json(new
                    {
                        success = true,
                        url = "/Restaurant/Ticket/Index",
                        message = "Message Sent Successfully",
                        data = Result,
                        creationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt")
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Message Sending Failed!",
                    });
                }

            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    url = "/Admin/Ticket/Index",
                    success = false,
                    message = err.Message
                });
            }
            return Json(new
            {
                success = false,
                message = "Fill all required fields and submit the form again"
            });
        }
        public async Task<IActionResult> StatusChange(long Id)
        {
            TicketViewModel ticket = _mapper.Map<TicketViewModel>(await _client.GetTicket(Id));
            TempData["TicketID"] = Id;
            return PartialView(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> StatusChange(TicketViewModel ticket, string status)
        {
            TicketViewModel ticketmodel = _mapper.Map<TicketViewModel>(await _client.GetTicket(ticket.Id));
            ticketmodel.Status = status;
            var model = await _client.UpdateStatus(ticketmodel);
            if (model != null)
            {
                return Json(new
                {
                    success = true,
                    url = "/Restaurant/Ticket/Index",
                    message = "Status updated successfully ...",
                    data = new
                    {
                        ID = ticketmodel.Id,
                        Description = ticketmodel.Description,
                        Priority = ticketmodel.Priority,
                        Status = ticketmodel.Status,
                        Date = ticketmodel.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                        TicketNo = ticketmodel.TicketNo == null ? "-" : ticketmodel.TicketNo,


                    }
                });
            }
            else

                return Json(new
                {
                    success = false,
                    message = "Ooops! something went wrong..."
                });
        }
    }
}
