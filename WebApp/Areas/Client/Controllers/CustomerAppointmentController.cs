using AutoMapper;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using System.Linq;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class CustomerAppointmentController : Controller
    {
        private readonly IGarageCustomerAppointmentClient _CustomerAppointmentClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public CustomerAppointmentController(IGarageCustomerAppointmentClient customerAppointmentClient, IMapper mapper, IUserSessionManager userSession)
        {
            _CustomerAppointmentClient = customerAppointmentClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageCustomerAppointmentViewModel>>(await _CustomerAppointmentClient.GetAllByGarageIdAsync(GarageId)));
        }
        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<GarageCustomerAppointmentViewModel> result = _mapper.Map<IEnumerable<GarageCustomerAppointmentViewModel>>(await _CustomerAppointmentClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
    }
}
