using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Garage.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "GarageOwner")]
    public class GarageController : Controller
    {
        private readonly IUserClient _userClient;
        private readonly IGarageClient _garageClient;
        private readonly IGarageImageClient _garageImageClient;
        private readonly IGarageRepairClient _garageRepairClient;
        private readonly IGarageScheduleClient _garageScheduleClient;
        private readonly ICarMakeClient _CarMakeclient;
        private readonly IMapper _mapper;

        public GarageController(IGarageClient garageClient, IMapper mapper, ICarMakeClient carMakeclient, IGarageImageClient garageImageClient, IGarageRepairClient garageRepairClient, IGarageScheduleClient garageScheduleClient, IUserClient userClient)
        {
            _userClient = userClient;
            _garageClient = garageClient;
            _mapper = mapper;
            _CarMakeclient = carMakeclient;
            _garageImageClient = garageImageClient;
            _garageRepairClient = garageRepairClient;
            _garageScheduleClient = garageScheduleClient;
        }
        
        [Route("Client/Profile")]
        public async Task<IActionResult> Edit()
        {
            ViewBag.CarMake = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _CarMakeclient.GetAllCarMakesAsync());
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByUser());
            return View(garage);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GarageViewModel model)
        {
            GarageRegisterDTO garage = new();
            garage.PhoneNumber = model.User.PhoneNumber;
            garage.Password = model.User.Password;
            garage.Email = model.User.Email;
            garage.Garage = _mapper.Map<GarageDTO>(model);
            object Result = await _garageClient.Edit(garage);
            
            //UserDTO user = new();
            //user.Email = model.User.Email;
            //user.PhoneNumber = model.User.PhoneNumber;
            //object Result2 = await _userClient.UpdateUserAsync(user);
            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
            });
        }

        public async Task<IActionResult> Details(long Id)
        {
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            return View(garage);
        }
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason)
        {
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.ToggleActiveStatus(Id, flag, RejectionReason));
            return Json(new
            {
                success = true,
                message = "Status Updated Successfully!",
                data = new
                {
                    Id = garage.Id,
                    creationDate = garage.CreationDate.ToString("dd MMM yyyy, h: mm tt"),
                    nameAsPerTradeLicense = garage.NameAsPerTradeLicense,
                    logo = garage.Logo.Replace(" ", "%20"),
                    contactPersonName = garage.ContactPersonName,
                    contactPersonEmail = garage.ContactPersonEmail,
                    contactPersonNumber = "+971 " + garage.ContactPersonNumber,
                    status = garage.Status
                }
            });
        }
        
       
        public IActionResult DocumentModel(long GarageId)
        {
            return View(GarageId);
        }
        public async Task<IActionResult> GetImages(long GarageId)
        {
            IEnumerable<GarageImageViewModel> Images = await _garageImageClient.GetByGarage(GarageId);
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ...",
                data = Images
            });
        }

        public async Task<IActionResult> AddRepairSpecification(long GarageId, long CarMakeId)
        {
            var result = await _garageRepairClient.Add(GarageId, CarMakeId);
            return Json(new
            {
                success = true,
                message = "Make Added successfully ...",
                data = result
            });
        }

        public async Task<IActionResult> DeleteRepairSpecification(long GarageId, long CarMakeId)
        {
            await _garageRepairClient.Delete(GarageId, CarMakeId);
            return Json(new
            {
                success = true,
                message = "Make removed successfully ...",
            });
        }
        public IActionResult Schedule(long Id)
        {
            ViewBag.GarageId = Id;
            return View();
        }

        public async Task<IActionResult> GetSchedule(long Id)
        {
            var schedule = await _garageScheduleClient.GetAllByGarage(Id);
            return Json(new
            {
                success = true,
                data = schedule,
                message = "Success"
            });
        }

        public async Task<IActionResult> SetSchedule(List<GarageScheduleViewModel> model)
        {
            if (model.Any())
            {
                await _garageScheduleClient.AddandUpdateGarageSchedule(_mapper.Map<List<GarageScheduleDTO>>(model));

                return Json(new
                {
                    success = true,
                    message = "Schedule Successfully Added.."
                });
            }

            return Json(new
            {
                success = false,
                message = "Record already exists!"
            });
        }

        public async Task<IActionResult> DeleteSchedule(long Id)
        {
            await _garageScheduleClient.Delete(Id);

            return Json(new
            {
                success = true,
                message = "Schedule Deleted Successfully"
            });
        }
    }
}
