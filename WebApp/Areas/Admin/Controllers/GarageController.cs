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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager,GarageOwner")]
    //[Route("Client/[action]")]
    public class GarageController : Controller
    {
        private readonly IGarageClient _garageClient;
        private readonly IClientIndustriesClient _industriesClient;
        private readonly IClientSectionsClient _sectionClient;
        private readonly IClientTypesClient _typesClient;
        private readonly IGarageImageClient _garageImageClient;
        private readonly IGarageRepairClient _garageRepairClient;
        private readonly IGarageScheduleClient _garageScheduleClient;
        private readonly ICarMakeClient _CarMakeclient;
        private readonly IMapper _mapper;

        public GarageController(IGarageClient garageClient, IMapper mapper, ICarMakeClient carMakeclient, IGarageImageClient garageImageClient, IGarageRepairClient garageRepairClient, IGarageScheduleClient garageScheduleClient, IClientIndustriesClient industriesClient, IClientSectionsClient sectionClient, IClientTypesClient typesClient)
        {
            _garageClient = garageClient;
            _mapper = mapper;
            _CarMakeclient = carMakeclient;
            _garageImageClient = garageImageClient;
            _garageRepairClient = garageRepairClient;
            _garageScheduleClient = garageScheduleClient;
            _industriesClient = industriesClient;
            _sectionClient = sectionClient;
            _typesClient = typesClient;
        }
        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<GarageViewModel>>(await _garageClient.GetGarages());
            return View(info);
        }
        public async Task<IActionResult> List()
        {
            var info = _mapper.Map<IEnumerable<GarageViewModel>>(await _garageClient.GetGarages());
            return View(info);
        }
        public async Task<IActionResult> Detail(long Id)
        {
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            return View(garage);
        }

        public async Task<IActionResult> Edit(long Id)
        {
            var ClientIndustry = _mapper.Map<IEnumerable<ClientIndustriesViewModel>>(await _industriesClient.GetIndustries());
            ViewBag.ClientIndustries = ClientIndustry.OrderBy(x => x.Name);
            var ClientSections = _mapper.Map<IEnumerable<ClientSectionsViewModel>>(await _sectionClient.GetCities());
            ViewBag.ClientSection = ClientSections.OrderBy(x => x.Name);
            var ClientType = _mapper.Map<IEnumerable<ClientTypesViewModel>>(await _typesClient.GetCities());
            ViewBag.ClientTypes = ClientType.OrderBy(x => x.Name);
            ViewBag.CarMake = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _CarMakeclient.GetAllCarMakesAsync());
            GarageViewModel garage = _mapper.Map<GarageViewModel>(await _garageClient.GetGarageByID(Id));
            return View(garage);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GarageViewModel model)
        {
            GarageRegisterDTO garage = new();
            garage.PhoneNumber = model.User.PhoneNumber;
            garage.Password = model.User.Password;
            garage.Garage = _mapper.Map<GarageDTO>(model);
            object Result = await _garageClient.Edit(garage);

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
                    creationDate = garage.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    nameAsPerTradeLicense = garage.NameAsPerTradeLicense,
                    logo = garage.Logo.Replace(" ", "%20"),
                    contactPersonName = garage.ContactPersonName,
                    contactPersonEmail = garage.ContactPersonEmail,
                    contactPersonNumber = "+971 " + garage.ContactPersonNumber,
                    status = garage.Status
                }
            });
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _garageClient.Delete(Id);

                return Json(new
                {
                    success = true,
                    message = "Record Deleted Successfully"
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
        public async Task<IActionResult> GarageMakeReport()
        {
            var info = _mapper.Map<List<GarageViewModel>>(await _garageClient.GetGarages());
            return new CSVResult<GarageViewModel>(info, "Garage");
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

        [HttpPost]
        public async Task<IActionResult> ThumbnailImage(long Id, string filePath)
        {
            string result = await _garageClient.UpdateThumbnail(Id, filePath);

            return Json(new
            {
                success = true,
                message = "Thumbnail updated successfully!",
                filepath = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> SecondaryLogo(long Id, string filePath)
        {
            string result = await _garageClient.UpdateSecondaryLogo(Id, filePath);

            return Json(new
            {
                success = true,
                message = "Logo updated successfully!",
                filepath = result
            });
        }
    }
}
