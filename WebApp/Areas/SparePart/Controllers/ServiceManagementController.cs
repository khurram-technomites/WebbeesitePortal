using AutoMapper;
using HelperClasses.Classes;
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
    public class ServiceManagementController : Controller
    {
        private readonly ISparePartServiceManagementClient _serviceManagementClient;
        private readonly IMapper _mapper;
        private readonly IUserSessionManager _userSession;


        public ServiceManagementController(ISparePartServiceManagementClient serviceManagementClient, IMapper mapper, IUserSessionManager userSession)
        {
            _serviceManagementClient = serviceManagementClient;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;
            return View(_mapper.Map<IEnumerable<SparePartServiceManagementViewModel>>(await _serviceManagementClient.GetAllBySparePartDealerIdAsync(SparePartId)));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SparePartServiceManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            Model.Status = Enum.GetName(typeof(Status), Status.Active);
            SparePartServiceManagementDTO result = await _serviceManagementClient.AddSparePartServiceManagementAsync(_mapper.Map<SparePartServiceManagementDTO>(Model));

            return Json(new
            {
                success = true,
                message = "Service added successfully!",
                data = new
                {

                    id = result.Id,
                    date = result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                    icon = result.Icon,
                    title = result.Title,
                    status = result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,



                }
            });
        }

        public async Task<IActionResult> Edit(long Id)
        {
            IEnumerable<SparePartServiceManagementViewModel> result = _mapper.Map<IEnumerable<SparePartServiceManagementViewModel>>(await _serviceManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartServiceManagementViewModel Model)
        {
            long SparePartId = _userSession.GetSparePartDealerStore().Id;

            Model.SparePartDealerId = SparePartId;
            SparePartServiceManagementDTO result = await _serviceManagementClient.UpdateSparePartServiceManagementAsync(_mapper.Map<SparePartServiceManagementDTO>(Model));

            return Json(new
            {
                success = true,
                message = "Service Updated successfully!",
                data = new
                {

                    id = result.Id,
                    date = result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                    icon = result.Icon,
                    title = result.Title,
                    status = result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,



                }
            });
        }

        public async Task<IActionResult> Detail(long Id)
        {
            IEnumerable<SparePartServiceManagementViewModel> result = _mapper.Map<IEnumerable<SparePartServiceManagementViewModel>>(await _serviceManagementClient.GetAllByIdAsync(Id));
            return View(result.FirstOrDefault());
        }

        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                await _serviceManagementClient.DeleteSparePartServiceManagementAsync(Id);

                return Json(new
                {
                    success = true,
                    message = "Service Deleted Successfully"
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

        public async Task<ActionResult> ToggleActiveStatus(long Id)
        {
            try
            {
                SparePartServiceManagementViewModel Result = _mapper.Map<SparePartServiceManagementViewModel>(await _serviceManagementClient.ToggleStatus(Id));
                return Json(new
                {
                    success = true,
                    message = "Status Updated Successfully",
                    data = new
                    {
                        Date = Result.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                        Icon = Result.Icon + "|" + Result.Title,
                        Status = Result.Status == Enum.GetName(typeof(Status), Status.Active) ? true : false,
                        ID = Result.Id
                    }

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
