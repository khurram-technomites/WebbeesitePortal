using AutoMapper;
using Fingers10.ExcelExport.ActionResults;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ErrorHandling;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Automobile Manager")]
    public class SparePartsDealerController : Controller
    {
        private readonly ISparePartsDealerClient _sparePartsDealerClient;
        private readonly ISparePartsDocumentClient _sparePartsDocumentClient;
        private readonly ISparePartsDealerScheduleClient _sparePartsScheduleClient;
        private readonly ISparePartsDealerInventorySpecificationClient _sparePartsDealerInventorySpecificationClient;
        private readonly ICountryClient _countryClient;
        readonly ICityClient _cityClient;
        private readonly IUserClient _userClient;
        private readonly ICarMakeClient _makeClient;
        private readonly ISparePartsDealerImageClient _imageClient;
        private readonly IMapper _mapper;

        public SparePartsDealerController(ISparePartsDealerClient sparePartsDealerClient,
            ICountryClient countryClient,
            ICityClient cityClient,
            ICarMakeClient makeClient, ISparePartsDocumentClient sparePartsDocumentClient,
            ISparePartsDealerScheduleClient sparePartsScheduleClient,
            IMapper mapper,
            IUserClient userClient,
            ISparePartsDealerImageClient imageClient,
            ISparePartsDealerInventorySpecificationClient sparePartsDealerInventorySpecificationClient)
        {
            _sparePartsDealerClient = sparePartsDealerClient;
            _mapper = mapper;
            _sparePartsDocumentClient = sparePartsDocumentClient;
            _makeClient = makeClient;
            _userClient = userClient;
            _sparePartsScheduleClient = sparePartsScheduleClient;
            _sparePartsDealerInventorySpecificationClient = sparePartsDealerInventorySpecificationClient;
            _countryClient = countryClient;
            _cityClient = cityClient;
            _imageClient = imageClient;
        }

        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());
            return View(info);
        }
        public async Task<IActionResult> List()
        {
            var info = _mapper.Map<IEnumerable<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());
            return PartialView(info);
        }
        public async Task<IActionResult> Details(long Id)
        {
            SparePartsDealerViewModel sparePartsDealerViewModel = _mapper.Map<SparePartsDealerViewModel>(await _sparePartsDealerClient.GetSparePartsDealerByID(Id));
            return View(sparePartsDealerViewModel);
        }
        public async Task<IActionResult> Detail(long Id)
        {
            SparePartsDealerViewModel sparePartsDealerViewModel = _mapper.Map<SparePartsDealerViewModel>(await _sparePartsDealerClient.GetSparePartsDealerByID(Id));
            return View(sparePartsDealerViewModel);
        }
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason)
        {
            SparePartsDealerViewModel sparePartsDealerViewModel = _mapper.Map<SparePartsDealerViewModel>(await _sparePartsDealerClient.ToggleActiveStatus(Id, flag, RejectionReason));
            return Json(new
            {
                success = true,
                message = "Status Updated Successfully!",
                data = new
                {
                    Id = sparePartsDealerViewModel.Id,
                    CreationDate = sparePartsDealerViewModel.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    NameAsPerTradeLicense = sparePartsDealerViewModel.Logo.Replace(" ", "%20") + "|" + sparePartsDealerViewModel.NameAsPerTradeLicense + "|" + sparePartsDealerViewModel.ContactPersonEmail,
                    ContactPersonName = sparePartsDealerViewModel.ContactPersonName,
                    ContactPersonEmail = sparePartsDealerViewModel.ContactPersonEmail,
                    ContactPersonNumber = "+971 " + sparePartsDealerViewModel.ContactPersonNumber,
                    Status = sparePartsDealerViewModel.Status
                }
            });
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _sparePartsDealerClient.Delete(Id);

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
        public async Task<IActionResult> SparePartsDealerMakeReport()
        {
            var info = _mapper.Map<List<SparePartsDealerViewModel>>(await _sparePartsDealerClient.GetSparePartsDealers());
            return new CSVResult<SparePartsDealerViewModel>(info, "SparePartsDealer");
        }
        public async Task<IActionResult> Edit(long Id)
        {
            var Info = _mapper.Map<IEnumerable<CarMakeViewModel>>(await _makeClient.GetAllCarMakesAsync());
            ViewBag.CarMake = (IEnumerable<CarMakeViewModel>)Info;
            SparePartsDealerViewModel sparePartsDealerViewModel = _mapper.Map<SparePartsDealerViewModel>(await _sparePartsDealerClient.GetSparePartsDealerByID(Id));
            ViewBag.UserId = sparePartsDealerViewModel.UserId;
            return View(sparePartsDealerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SparePartsDealerViewModel model, string Logo)
        {
            try
            {
                //logoPath = model.Logo;
                var user = await _userClient.GetUserByIdAsync(model.UserId);
                SparePartsDealerRegisterDTO spareParts = new();
                spareParts.PhoneNumber = model.User.PhoneNumber;
                spareParts.Password = model.User.Password;
                spareParts.SparePartsDealer = _mapper.Map<SparePartsDealerDTO>(model);
                var result = await _sparePartsDealerClient.Update(spareParts);
                return Json(new
                {
                    success = true,
                    message = "Dealer updated successfully",
                    data = model.Id
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
        //[HttpPost]
        public async Task<IActionResult> GetImages(long SparePartsDealerId)
        {
            IEnumerable<SparePartDealerImagesViewModel> Images = await _imageClient.GetBySpareParts(SparePartsDealerId);
            return Json(new
            {
                success = true,
                message = "Document deleted successfully ...",
                data = Images
            });
        }
        public IActionResult Schedule(long Id)
        {
            ViewBag.SparePartsDealerId = Id;
            return View();
        }
        public async Task<IActionResult> GetSchedule(long Id)
        {
            var schedule = await _sparePartsScheduleClient.GetAllSparePartsDealerSchedulesAsync(Id);

            //if (schedule.Any())
            //{
            return Json(new
            {
                success = true,
                data = schedule,
                message = "Success"
            });
        }
        public async Task<IActionResult> SetSchedule(List<SparePartsDealerScheduleViewModel> model)
        {
            if (model.Any())
            {
                foreach (var item in model)
                {
                    if (item.Id != 0)
                    {
                        await _sparePartsScheduleClient.UpdateSparePartsDealerScheduleAsync(_mapper.Map<SparePartsDealerScheduleDTO>(item));
                    }
                    else
                    {
                        await _sparePartsScheduleClient.AddSparePartsDealerScheduleAsync(_mapper.Map<SparePartsDealerScheduleDTO>(item));
                    }
                }

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
            await _sparePartsScheduleClient.DeleteSparePartsDealerScheduleAsync(Id);

            return Json(new
            {
                success = true,
                message = "Schedule Deleted Successfully"
            });
        }

        //return Json(new
        //{
        //    success = false,
        //    message = "No shchedule found.. !"
        //});

        public IActionResult DocumentModel(long Id)
        {
            return View(Id);
        }

        public async Task<IActionResult> DeleteDocument(long Id)
        {
            await _sparePartsDocumentClient.DeleteDocument(Id);

            return Json(
                new
                {
                    success = true,
                    message = "Document Deleted Successfully",
                });
        }
        [HttpPost]
        public async Task<IActionResult> AddDocument(SparePartsDealerDocumentViewModel Model)
        {
            var result = await _sparePartsDocumentClient.AddDocument(_mapper.Map<SparePartsDealerDocumentDTO>(Model));

            return Json(
                new
                {
                    success = true,
                    message = "Document Added Successfully",
                    data = new
                    {
                        result.Id,
                        result.Path,
                        FormattedDate = result.FormattedDate,
                        result.Type
                    }
                });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCarMake(SparePartsDealerSpecificationsViewModel model)
        {
            var carMake = await _sparePartsDealerInventorySpecificationClient.GetSparePartsDealerInventorySpecificationByCarMakeID(model.CarMakeId);
            if (carMake == null)
            {
                var result = await _sparePartsDealerInventorySpecificationClient.AddSparePartsDealerInventorySpecification(_mapper.Map<SparePartsDealerSpecificationsDTO>(model));

                return Json(
                    new
                    {
                        success = true,
                        message = "Make Added Successfully",
                        data = new
                        {
                            result.Id,
                            result.CarMakeId,
                            result.SparePartsDealerId
                        }
                    });

            }
            else
            {
                await _sparePartsDealerInventorySpecificationClient.DeleteSparePartsDealerInventorySpecification(carMake.Id);

                return Json(
                    new
                    {
                        success = true,
                        message = "Make Deleted Successfully",
                    });
            }

        }
    }
}
