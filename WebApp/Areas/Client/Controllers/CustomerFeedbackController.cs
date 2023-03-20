using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class CustomerFeedbackController : Controller
    {
        private readonly IGarageCustomerFeedbackClient _CustomerFeedbackClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public CustomerFeedbackController(IGarageCustomerFeedbackClient customerFeedbackClient, IMapper mapper, IUserSessionManager userSession)
        {
            _CustomerFeedbackClient = customerFeedbackClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            return View(_mapper.Map<IEnumerable<GarageCustomerFeedbackViewModel>>(await _CustomerFeedbackClient.GetAllByGarageIdAsync(GarageId)));
        }
    }
}
