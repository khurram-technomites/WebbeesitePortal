using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class SubscriberController : Controller
    {
        private readonly IGarageSubscribersClient _subService;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;

        public SubscriberController(IGarageSubscribersClient subService, IMapper mapper, IUserSessionManager userSession)
        {
            _subService = subService;
            _mapper = mapper;
            _userSession = userSession;
        }
        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            var info = _mapper.Map<IEnumerable<GarageSubscribersViewModel>>(await _subService.GetAllByGarageIdAsync(GarageId));
            return View(info);
        }
        public async Task<IActionResult> MakeReport()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            var info = _mapper.Map<IEnumerable<GarageSubscribersViewModel>>(await _subService.GetAllByGarageIdAsync(GarageId));
            return new CSVResult<GarageSubscribersViewModel>(info, "Subscribers");
        }
    }

}
