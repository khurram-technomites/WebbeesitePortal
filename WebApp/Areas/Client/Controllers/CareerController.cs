using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize("GarageOwner")]
    public class CareerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSessionManager;
        private readonly IGarageCareersClient _careersClient;

        public CareerController(IMapper mapper, IUserSessionManager userSessionManager, IGarageCareersClient garageCareers)
        {
            _mapper = mapper;
            _userSessionManager = userSessionManager;
            _careersClient = garageCareers;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSessionManager.GetGarageStore().Id;
            IEnumerable<GarageCareersViewModel> result = _mapper.Map<IEnumerable<GarageCareersViewModel>>(await _careersClient.GetAllByGarageIdAsync(GarageId));
            return View(result);
        }

        public async Task<IActionResult> MakeReport()
        {
            long GarageId = _userSessionManager.GetGarageStore().Id;
            IEnumerable<GarageCareersViewModel> result = _mapper.Map<IEnumerable<GarageCareersViewModel>>(await _careersClient.GetAllByGarageIdAsync(GarageId));
            return new CSVResult<GarageCareersViewModel>(result, "Careers");
        }
        //[HttpPost]
        //public IActionResult DownloadContent(string path = "")
        //{
        //    return File();
        //}

    }
}
