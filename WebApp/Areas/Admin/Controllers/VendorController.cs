using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "Admin, Automobile Manager,GarageOwner")]
    public class VendorController : Controller
    {
        private readonly IVendorClient _vendorClient;
        private readonly IMapper _mapper;
        private readonly ICityClient _cityService;
        private readonly ICountryClient _countryService;
        private readonly IVendorDocumentClient _vendorDocumentClient;
        public VendorController(IVendorClient vendorClient, IMapper mapper, ICityClient cityService, ICountryClient countryService,IVendorDocumentClient vendorDocumentClient)
        {
            _vendorClient = vendorClient;
            _mapper = mapper;
            _cityService = cityService;
            _countryService = countryService;
            _vendorDocumentClient = vendorDocumentClient;

        }
        public async Task<IActionResult> Index()
        {
            var info = _mapper.Map<IEnumerable<VendorViewModel>>(await _vendorClient.GetVendors());
            return View(info.Where(x=>x.Status != Enum.GetName(typeof(Status), Status.Pending) && x.Status != Enum.GetName(typeof(Status),Status.Rejected)));
        }
        public async Task<IActionResult> Detail(long Id)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(Id));
            return View(vendor);
        }
        public async Task<IActionResult> Create()
        {
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(VendorViewModel model)
        {
            if (ModelState.IsValid)
            {
                object Result = await _vendorClient.Create(model);
                if (Result != null)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Vendor Added Successfully",
                        data = Result,
                        url = "/Admin/Vendor/Index"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "An error occured",
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Please Fill Form ",
                });

            }
            


        }
        public async Task<IActionResult> Edit(long Id)
        {
            var countries = _mapper.Map<IEnumerable<CountryViewModel>>(await _countryService.GetCountries());
            ViewBag.Country = countries.OrderBy(x => x.Name);
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(Id));
            vendor.VendorDocuments = (List<VendorDocumentViewModel>)await _vendorDocumentClient.GetAllByVendor(vendor.Id);
            return View(vendor);
        }
        public async Task<IActionResult> GetCityBycountryId(long countryId)
        {
            var cities = await _cityService.GetCityByCountryId(countryId);

            return Json(new
            {
                success = true,
                data = cities
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VendorViewModel model)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(model.Id));
            model.Email = vendor.Email;
            model.NameArAsPerTradeLicense = vendor.NameArAsPerTradeLicense;
            model.UserId = vendor.UserId;
            object Result = await _vendorClient.Edit(model);

            return Json(new
            {
                success = true,
                message = "Record Updated Successfully",
                data = Result
            });
        }
        public IActionResult DocumentModel(long GarageId)
        {
            return View(GarageId);
        }
        public async Task<IActionResult> Details(long Id)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.GetVendorByID(Id));
            return View(vendor);
        }
        public async Task<IActionResult> ToggleActiveStatus(long Id, bool flag, string RejectionReason)
        {
            VendorViewModel vendor = _mapper.Map<VendorViewModel>(await _vendorClient.ToggleActiveStatus(Id, flag, RejectionReason));
            return Json(new
            {
                success = true,
                message = "Status Updated Successfully!",
                data = new
                {
                    Id = vendor.Id,
                    creationDate = vendor.CreationDate.ToString("dd MMM yyyy, hh:mm tt"),
                    nameAsPerTradeLicense = vendor.NameAsPerTradeLicense,
                    contactPersonName = vendor.ContactNumber1,
                    contactPersonEmail = vendor.Email,
                    contactPersonNumber = vendor.ContactNumber1,
                    status = vendor.Status
                }
            });
        }
        public async Task<ActionResult> Delete(long Id)
        {
            try
            {
                await _vendorClient.Delete(Id);

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
    }
}
