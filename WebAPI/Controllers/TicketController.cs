using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly INumberRangeService _numberRangeService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IRestaurantService _restaurantService;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;
        public TicketController(ITicketService ticketService, IMapper mapper, IFTPUpload fTPUpload , INumberRangeService numberRangeService, INotificationService notificationService, 
            UserManager<AppUser> userManager
            , IRestaurantService restaurantService
            , ISupplierService supplierService)
        {
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _notificationService = notificationService;
            _ticketService = ticketService;
            _numberRangeService = numberRangeService;
            _userManager = userManager;
            _restaurantService = restaurantService;
            _supplierService = supplierService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetAllTicketsAsync()));
        }

        [HttpGet("User/{UserId}")]
        public async Task<IActionResult> GetTicketByUserId(string UserId)
        {
            return Ok(_mapper.Map<TicketDTO>(await _ticketService.GetTicketsByUser(UserId)));
        }
        [HttpGet("Restaurant/{RestaurantId}")]
        public async Task<IActionResult> GetTicketByRestaurantId(long RestaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsByRestaurant(RestaurantId)));
        }
        [HttpGet("Supplier/{SupplierId}")]
        public async Task<IActionResult> GetTicketBySupplierId(long SupplierId)
        {
            return Ok(_mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsBySupplier(SupplierId)));
        }
        [HttpGet("Supplier/List/{Module}")]
        public async Task<IActionResult> GetListOfSupplierByModule(string Module)
        {
            var list = _mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsByModule(Module));
            if (Module == "Supplier")
            {
                var supplier = _mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsByModule(Module)).Where(x => x.SupplierId != null);
                return Ok(supplier);
            }
            else if (Module == "Restaurant")
            {
                var restaurant = _mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsByModule(Module)).Where(x => x.RestaurantId != null);
                return Ok(restaurant);
            }
            else
            {
                return Ok(list);
            }
        }
        [HttpGet("Status")]
        public async Task<IActionResult> GetTicketByOpenStatus()
        {
            return Ok(_mapper.Map<IEnumerable<TicketDTO>>(await _ticketService.GetTicketsByOpenStatus()));
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            return Ok(_mapper.Map<TicketDTO>(await _ticketService.GetTicket(Id)));
        }
        [HttpPost]
        public async Task<IActionResult> Add(TicketDTO Model)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.TicketNo = await _numberRangeService.GetNextRange("TKT-");
            Model.Status = "OPEN";
            var adminID = await _userManager.FindByEmailAsync("admin@fougito.com");
            //AppUser User = await _userManager.FindByIdAsync(UserId);
            Model.user = null;
            Model.Restaurant = null;
            TicketDTO dTO =  _mapper.Map<TicketDTO>(await _ticketService.Create(_mapper.Map<Ticket>(Model)));
            if (Model.Restaurant != null)
            {
                IEnumerable<Models.Restaurant> List = await _restaurantService.GetRestaurantByIdAsync(Model.RestaurantId);
                Models.Restaurant restaurant = List.FirstOrDefault();
                NotificationDTO notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = restaurant.NameArAsPerTradeLicense,
                    Description = "New Ticket Created",
                    RecordId = dTO.Id,
                    Title = "Ticket",
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                    Url = "/Admin/Ticket/Index",
                    NotificationReceivers = new List<NotificationReceiverDTO>()
                    {
                        new NotificationReceiverDTO
                        {
                            ReceiverId = adminID.Id,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Admin),
                        },
                         new NotificationReceiverDTO
                        {
                            ReceiverId = UserId,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Restaurant),
                        },
                    },
                };
                await _notificationService.AddNotification(_mapper.Map<Notification>(notification));
            }
            else if (Model.Supplier != null)
            {
                IEnumerable<Models.Supplier> List = await _supplierService.GetByIdAsync(Model.SupplierId.Value);
                Models.Supplier supplier = List.FirstOrDefault();
                NotificationDTO notification = new()
                {
                    OriginatorId = UserId,
                    OriginatorName = supplier.NameAsPerTradeLicense,
                    Description = "New Ticket Created",
                    RecordId = dTO.Id,
                    Title = "Ticket",
                    OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
                    Url = "/Admin/Ticket/Index",
                    NotificationReceivers = new List<NotificationReceiverDTO>()
                    {
                        new NotificationReceiverDTO
                        {
                            ReceiverId = adminID.Id,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Admin),
                        },
                         new NotificationReceiverDTO
                        {
                            ReceiverId = UserId,
                            IsSeen = false,
                            IsDelivered = false,
                            IsRead = false,
                            ReceiverType = Enum.GetName(typeof(Logins), Logins.Restaurant),
                        },
                    },
                };
                await _notificationService.AddNotification(_mapper.Map<Notification>(notification));
            }
            

            //NotificationDTO notification = new()
            //{
            //    OriginatorId = UserId,
            //    OriginatorName = restaurant.NameArAsPerTradeLicense,
            //    Description = "New Ticket Created",
            //    RecordId = dTO.Id,
            //    Title = "Ticket",
            //    OriginatorType = Enum.GetName(typeof(Logins), Logins.Customer),
            //    Url = "/Admin/Ticket/Index",
            //    NotificationReceivers = new List<NotificationReceiverDTO>()
            //        {
            //            new NotificationReceiverDTO
            //            {
            //                ReceiverId = adminID.Id,
            //                IsSeen = false,
            //                IsDelivered = false,
            //                IsRead = false,
            //                ReceiverType = Enum.GetName(typeof(Logins), Logins.Admin),
            //            },
            //             new NotificationReceiverDTO
            //            {
            //                ReceiverId = UserId,
            //                IsSeen = false,
            //                IsDelivered = false,
            //                IsRead = false,
            //                ReceiverType = Enum.GetName(typeof(Logins), Logins.Restaurant),
            //            },
            //        },
            //};
            
            return Ok(dTO);
        }
        [HttpPut]
        public async Task<IActionResult> Put(TicketDTO Model)
        {

            var ticket =  _mapper.Map<TicketDTO>(await _ticketService.GetTicket(Model.Id));
            ticket.UserId = Model.UserId;
            return Ok(_mapper.Map<TicketDTO>(await _ticketService.Update(_mapper.Map<Ticket>(ticket))));
        }
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(TicketDTO Model)
        {

            var ticket = _mapper.Map<TicketDTO>(await _ticketService.GetTicket(Model.Id));
            ticket.Status = Model.Status;
            return Ok(_mapper.Map<TicketDTO>(await _ticketService.Update(_mapper.Map<Ticket>(ticket))));
        }
        //[HttpGet("ToggleStatus/{Id}")]
        //public async Task<IActionResult> ToggleStatus(long Id)
        //{
        //    TicketDTO make = await _ticketService.GetTicket(Id);

        //    if (make.Status == Enum.GetName(typeof(Status), Status.Active))
        //        make.Status = Enum.GetName(typeof(Status), Status.Inactive);
        //    else
        //        make.Status = Enum.GetName(typeof(Status), Status.Active);

        //    return Ok(await _ticketService.(make));
        //}
    }
}
