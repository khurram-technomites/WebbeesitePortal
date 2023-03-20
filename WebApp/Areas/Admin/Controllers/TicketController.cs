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
using HelperClasses.Classes;
using WebAPI.Models;
using WebApp.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TicketController : Controller
    {
        private readonly ITicketClient _ticketService;
        private readonly ITicketMessageClient _ticketmessageService;
        private readonly IUserClient _userService;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;

        [BindProperty]
        public TicketViewModel Model { get; set; }
        public TicketController(ITicketClient ticketService, IUserSessionManager userSession, ITicketMessageClient ticketmessageService, IMapper mapper, IUserClient userService,
            IFileUpload fileUpload)
        {

            _ticketService = ticketService;
            _ticketmessageService = ticketmessageService;
            _mapper = mapper;
            _userService = userService;
            _userSession = userSession;
            _fileUpload = fileUpload;

        }
        public async Task<ActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<TicketViewModel>>(await _ticketService.GetAllTicketsAsync());
            return View(info);
        }
        public async Task<ActionResult> List(string status)
        {
            var model = _mapper.Map<IEnumerable<TicketViewModel>>(await _ticketService.GetAllTicketsByModuleAsync(status));
            return PartialView(model);
        }
        public async Task<IActionResult> Details(long Id)
        {
            TicketViewModel Details = new TicketViewModel();
            IEnumerable<TicketMessagesDTO> conversation = await _ticketmessageService.GetTicketMessageByTicketId(Id);
            TicketViewModel a = _mapper.Map<TicketViewModel>(await _ticketService.GetTicket(Id));
            a.ticketConversation = _mapper.Map<List<TicketMessageViewModel>>(conversation);
            return View(a);
        }
        public async Task<IActionResult> StatusChange(long Id)
        {
            TicketViewModel ticket = _mapper.Map<TicketViewModel>(await _ticketService.GetTicket(Id));
            TempData["TicketID"] = Id;
            return PartialView(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> StatusChange(TicketViewModel ticket, string status)
        {
            TicketViewModel ticketmodel = _mapper.Map<TicketViewModel>(await _ticketService.GetTicket(ticket.Id));
            ticketmodel.Status = status;
            var model = await _ticketService.UpdateStatus(ticketmodel);
            if (model != null)
            {
                return Json(new
                {
                    success = true,
                    url = "/Admin/Ticket/Index",
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
        public async Task<IActionResult> AssignUser(long? Id)
        {
            TempData["TicketID"] = Id;
            ViewBag.UserID = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "LastName");
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(string UserID, long Id)
        {
            TicketViewModel model = new TicketViewModel();
            model.UserId = UserID;
            model.Id = Id;
            //long ticketID = (long)TempData["TicketID"];
            //var ticket = _ticketService.GetTicket(Id);
            var ticketmodel = await _ticketService.UpdateTicket(model);

            if (ticketmodel != null)
            {


                return Json(new
                {
                    success = true,
                    url = "/Admin/Ticket/Index",
                    message = "Ticket assign successfully ...",
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
            {
                return Json(new
                {
                    success = false,
                    message = "Ops something went wrong!"
                });
            }

            return Json(new
            {
                success = false,
                message = "Ops something went wrong!"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Message(TicketMessageViewModel model, string File)
        {
            //string message = string.Empty;
            model.SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.SenderType = User.FindFirstValue(ClaimTypes.Role);
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
                        url = "/Admin/Ticket/Index",
                        message = "Message Sent Successfully",
                        data = Result,
                        creationDate = Result.CreationDate.ToString("dd MMM yyyy, hh:mm tt")
                    });

                }
                else if (model.Message == null && File != "undefined" && File != null)
                {
                    model.CreationDate = DateTime.UtcNow;
                    model.Message = string.Empty;
                    TicketMessagesDTO Result = await _ticketmessageService.CreateTicketMessage(model);


                    return Json(new
                    {
                        success = true,
                        url = "/Admin/Ticket/Index",
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
    }
}
