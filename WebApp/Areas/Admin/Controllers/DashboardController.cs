using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin, Automobile Manager, Restaurant Manager, B2B Manager")]
    public class DashboardController : Controller
    {
        private readonly IDashboardClient _Service;
        private readonly IGarageClient _garageClient;
        private readonly ISparePartsDealerClient _sparePartsDealerClient;
        private readonly IMapper _mapper;
        [BindProperty]
        public DashboardViewModel Model { get; set; }
        public DashboardController(IDashboardClient Service, IMapper mapper, ISparePartsDealerClient sparePartsDealerClient, IGarageClient garageClient)
        {
            _Service = Service;
            _sparePartsDealerClient = sparePartsDealerClient;
            _garageClient = garageClient;
            _mapper = mapper;
        }
        public async Task<ActionResult> Index()
        {
            var count = _mapper.Map<DashboardViewModel>(await _Service.GetAdminDashboardCount());
            count.Garages = _mapper.Map<List<GarageViewModel>>(await _garageClient.GetGarages());
            count.SparePartsDealers = _mapper.Map<List<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());

            return View(count);
        }
    }
}
