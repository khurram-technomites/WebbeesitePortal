using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.NotificationFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers
{
    [Route("api/Admin/Notification")]
    [ApiController]
    [Authorize]
    public class AdminNotificationController : ControllerBase
    {
        private readonly INotificationRecieverService _service;
        private readonly IMapper _mapper;

        public AdminNotificationController(INotificationRecieverService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("GetAll/ByUser")]
        public async Task<IActionResult> GetAll(NotificationFilterDTO Filter)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Filter.UserId = UserId;
            return Ok(_mapper.Map<IEnumerable<NotificationReceiverDTO>>(await _service.GetAllNotificationsByUser(Filter)));
        }

        [HttpPut("MarkAllSeen/ByUser")]
        public async Task<IActionResult> MarkAllSeen()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(_mapper.Map<IEnumerable<NotificationReceiverDTO>>(await _service.MarkAllSeenByUser(UserId)));
        }

        [HttpPut("MarkRead/{NotificationId}")]
        public async Task<IActionResult> MarkRead(long NotificationId)
        {
            return Ok(_mapper.Map<NotificationReceiverDTO>(await _service.MarkReadById(NotificationId)));
        }

        [HttpGet("NewNotificationCount/ByUser")]
        public async Task<IActionResult> Count()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _service.GetNewNotificationsCountByUser(UserId));
        }
    }
}
