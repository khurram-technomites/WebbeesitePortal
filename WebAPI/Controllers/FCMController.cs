using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FCMController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFCMUserSessionService _fcmService;

        public FCMController(IMapper mapper, IFCMUserSessionService fcmService)
        {
            _mapper = mapper;
            _fcmService = fcmService;
        }

        [HttpPost]
        public async Task<IActionResult> FCM(FCMUserSessionDTO Model)
        {
            FCMUserSession mapResult = _mapper.Map<FCMUserSession>(Model);

            mapResult.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            mapResult.UserRole = User.FindFirstValue(ClaimTypes.Role);
            mapResult.User = null;
            return Ok(new SuccessResponse<FCMUserSessionDTO>("", _mapper.Map<FCMUserSessionDTO>(await _fcmService.AddUserSession(mapResult))));
        }
    }
}
