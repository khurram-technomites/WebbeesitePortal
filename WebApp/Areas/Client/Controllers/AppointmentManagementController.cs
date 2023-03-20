using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces.TypedClients;
using WebApp.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.ViewModels;
using System.Linq;
using HelperClasses.Classes;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.DTOs.GarageCMS;
using WebApp.Services.TypedClients;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class AppointmentManagementController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IGarageAppointmentManagementClient _garageAppointmentManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        public AppointmentManagementController(IGarageClient garageClient, IGarageAppointmentManagementClient garageAppointmentManagementClient, IUserSessionManager userSession, IMapper mapper)
        {
            _garageClient = garageClient;
            _garageAppointmentManagementClient = garageAppointmentManagementClient;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            long GarageId = _userSession.GetGarageStore().Id;
            IEnumerable<GarageAppointmentManagementViewModel> result = _mapper.Map<IEnumerable<GarageAppointmentManagementViewModel>>(await _garageAppointmentManagementClient.GetAllByGarageIdAsync(GarageId));

            if (result.Any())
                return View(result.FirstOrDefault());
            else
                return View(new GarageAppointmentManagementViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> AboutUsImage(string filePath)
        {
            long GarageId = _userSession.GetGarageStore().Id;

            IEnumerable<GarageAppointmentManagementDTO> content = await _garageAppointmentManagementClient.GetAllByGarageIdAsync(GarageId);

            if (content.Any())
            {
                content.FirstOrDefault().ImagePath = filePath;

                var result = await _garageAppointmentManagementClient.UpdateGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                GarageAppointmentManagementViewModel PostContent = new()
                {
                    ImagePath = filePath,
                    GarageId = GarageId
                };

                var result = await _garageAppointmentManagementClient.AddGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GarageAppointmentManagementViewModel Model)
        {
            long GarageId = _userSession.GetGarageStore().Id;
            Model.GarageId = GarageId;
            IEnumerable<GarageAppointmentManagementDTO> content = await _garageAppointmentManagementClient.GetAllByIdAsync(Model.Id);
            Model.ImagePath = content.FirstOrDefault().ImagePath;

            if (Model.Id == 0)
            {
                var result = await _garageAppointmentManagementClient.AddGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                var result = await _garageAppointmentManagementClient.UpdateGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }
    }
}
