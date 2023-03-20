using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.SparePart.Controllers
{
    [Area("SparePart")]
    [Authorize(Roles = "SparePartDealer")]
    public class SparePartTeamManagementController : Controller
    {
        private readonly ISparePartTeamManagementClient _serviceTeamManagementClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;
        public SparePartTeamManagementController(ISparePartTeamManagementClient serviceTeamManagementClient, IMapper mapper, IUserSessionManager userSession)
        {
            _serviceTeamManagementClient = serviceTeamManagementClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            return View(_mapper.Map<IEnumerable<SparePartTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllBySparePartDealerIdAsync(SparePartId)));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SparePartTeamManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartTeamManagementDTO result = await _serviceTeamManagementClient.AddSparePartTeamManagementAsync(_mapper.Map<SparePartTeamManagementDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartTeamManagement/Index",
                message = "Record Added Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.Name,
                    ImagePath = result.ImagePath,
                    Designation = result.Designation,
                    Id = result.Id,
                }
            });
        }
        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SparePartTeamManagementViewModel> result = _mapper.Map<IEnumerable<SparePartTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartTeamManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartTeamManagementDTO result = await _serviceTeamManagementClient.UpdateSparePartTeamManagementAsync(_mapper.Map<SparePartTeamManagementDTO>(Model));

            return Json(new
            {
                success = true,
                url = "/SparePart/SparePartTeamManagement/Index",
                message = "Record Updated Successfully",
                data = new
                {
                    Date = result.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    Name = result.Name,
                    ImagePath = result.ImagePath,
                    Designation = result.Designation,
                    Id = result.Id,
                }
            });
        }

        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<SparePartTeamManagementViewModel> result = _mapper.Map<IEnumerable<SparePartTeamManagementViewModel>>(await _serviceTeamManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceTeamManagementClient.DeleteSparePartTeamManagementAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Team Deleted Successfully"
                });
            }
            catch (ApiException ex)
            {
                ErrorDetails err = JsonConvert.DeserializeObject<ErrorDetails>(ex.Message);
                return Json(new
                {
                    success = false,
                    message = err.Message
                });
            }
        }

    }
}
