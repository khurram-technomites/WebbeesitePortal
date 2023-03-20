using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.NotificationFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRecieverService _service;
        private readonly IMapper _mapper;

        public NotificationController(INotificationRecieverService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("GetAll/ByUser")]
        public async Task<IActionResult> GetAll(NotificationFilterDTO Filter)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Filter.UserId = UserId;

            if (Filter.Period == Enum.GetName(typeof(Period), Period.Today))
            {
                Filter.StartDateTime = DateTime.UtcNow.ToDubaiDateTime().StartOfDay();
                Filter.EndDateTime = DateTime.UtcNow.ToDubaiDateTime().EndOfDay();
            }

            return Ok(new SuccessResponse<IEnumerable<NotificationReceiverDTO>>("", _mapper.Map<IEnumerable<NotificationReceiverDTO>>(await _service.GetAllNotificationsByUser(Filter))));
        }

        [HttpPut("MarkAllSeen/ByUser")]
        public async Task<IActionResult> MarkAllSeen()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new SuccessResponse<IEnumerable<NotificationReceiverDTO>>("", _mapper.Map<IEnumerable<NotificationReceiverDTO>>(await _service.MarkAllSeenByUser(UserId))));
        }

        [HttpPut("MarkRead/{NotificationRecieverId}")]
        public async Task<IActionResult> MarkRead(long NotificationRecieverId)
        {
            return Ok(new SuccessResponse<NotificationReceiverDTO>("", _mapper.Map<NotificationReceiverDTO>(await _service.MarkReadById(NotificationRecieverId))));
        }

        [HttpGet("NewNotificationCount/ByUser")]
        public async Task<IActionResult> Count(string Period)
        {
            DateTime? StartDateTime = null, EndDateTime = null;
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Period == Enum.GetName(typeof(Period), HelperClasses.Classes.Period.Today))
            {
                StartDateTime = DateTime.UtcNow.ToDubaiDateTime().StartOfDay();
                EndDateTime = DateTime.UtcNow.ToDubaiDateTime().EndOfDay();
            }

            long count = await _service.GetNewNotificationsCountByUser(UserId, StartDateTime, EndDateTime);

            return Ok(new SuccessResponse<string>("", count.ToString()));
        }
    }
}
