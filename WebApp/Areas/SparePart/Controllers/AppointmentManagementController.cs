using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class AppointmentManagementController : Controller
    {
        private readonly ISparePartsDealerClient _SparePartDealerClient;
        private readonly ISparePartAppointmentManagementClient _SparePartAppointmentManagementClient;
        private readonly IUserSessionManager _userSession;
        private readonly IMapper _mapper;
        public AppointmentManagementController(ISparePartsDealerClient SparePartClient, ISparePartAppointmentManagementClient SparePartAppointmentManagementClient, IUserSessionManager userSession, IMapper mapper)
        {
            _SparePartDealerClient = SparePartClient;
            _SparePartAppointmentManagementClient = SparePartAppointmentManagementClient;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            IEnumerable<SparePartAppointmentManagementViewModel> result = _mapper.Map<IEnumerable<SparePartAppointmentManagementViewModel>>(await _SparePartAppointmentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId));

            if (result.Any())
                return View(result.FirstOrDefault());
            else
                return View(new SparePartAppointmentManagementViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> AboutUsImage(string filePath)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            IEnumerable<SparePartAppointmentManagementDTO> content = await _SparePartAppointmentManagementClient.GetAllBySparePartDealerIdAsync(SparePartId);

            if (content.Any())
            {
                content.FirstOrDefault().ImagePath = filePath;

                var result = await _SparePartAppointmentManagementClient.UpdateSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagementDTO>(content.FirstOrDefault()));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                SparePartAppointmentManagementViewModel PostContent = new()
                {
                    ImagePath = filePath,
                    SparePartDealerId = SparePartId
                };

                var result = await _SparePartAppointmentManagementClient.AddSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagementDTO>(PostContent));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartAppointmentManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            Model.SparePartDealerId = SparePartId;
            IEnumerable<SparePartAppointmentManagementDTO> content = await _SparePartAppointmentManagementClient.GetAllByIdAsync(Model.Id);
            Model.ImagePath = content.FirstOrDefault().ImagePath;

            if (Model.Id == 0)
            {
                var result = await _SparePartAppointmentManagementClient.AddSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagementDTO>(Model));

                return Json(new
                {
                    success = true,
                    message = "Record Updated Successfully!",
                    data = result.Id
                });
            }
            else
            {
                var result = await _SparePartAppointmentManagementClient.UpdateSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagementDTO>(Model));

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
